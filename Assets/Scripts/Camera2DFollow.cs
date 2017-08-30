using UnityEngine;

namespace UnitySampleAssets._2D
{

    public class Camera2DFollow : MonoBehaviour
    {

        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float minClampY = 0f;
        public float maxClampY = Mathf.Infinity;
        public float searchTime = 0f;
        private Transform objMinClampX;
        private Transform objMaxClampX;
        public float minClampX = 0f;
        public float maxClampX = 1f;
        public Vector3 coordsDerechaCam;
        public Vector3 coordsIzquierdaCam;
        public float offsetCamDerecha = 0f;
        public float offsetCamIzquierda = 0f;

        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;
        private Vector3 derechaCam;
        private Vector3 izquierdaCam;
        
        
        private void Awake()
        {
            if (target == null){
                   SetCameraTarget();
            }
            GetMaxClampX();
            GetMinClampX();

        }


        // Use this for initialization
        private void Start()
        {
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
            izquierdaCam.x = 0f;
            izquierdaCam.y = 0f;
            izquierdaCam.z = 0f;

        }

        // Update is called once per frame
        private void Update()
        {
            //si la camara no tiene target a seguir, lo busca de nuevo.
            if (target == null){
                FindPlayer();
                return;
            }


            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - lastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
            
            if (updateLookAheadTarget)
            {
                lookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward*offsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

            getRightCam();

            //Consigue las coordenadas del borde derecho e izquierdo de la camara.
            coordsDerechaCam = Camera.main.ScreenToWorldPoint(derechaCam);
            coordsIzquierdaCam = Camera.main.ScreenToWorldPoint(izquierdaCam);

            //Calcula el offset del centro de la camara a los bordes.
            offsetCamIzquierda = coordsIzquierdaCam.x - newPos.x;
            offsetCamDerecha = coordsDerechaCam.x - newPos.x;
            
            //Clampea la camara en el eje X y en el eje Y.
            newPos = new Vector3(Mathf.Clamp(newPos.x, minClampX - offsetCamIzquierda, maxClampX - offsetCamDerecha), Mathf.Clamp(newPos.y, minClampY, maxClampY), newPos.z);

            transform.position = newPos;

            lastTargetPosition = target.position;
        }

        //busca el player target para seguimiento de camara
        private void FindPlayer(){
            if(searchTime <= Time.time){
                GameObject result = GameObject.FindGameObjectWithTag("Player");
                if(result != null){
                    target = result.transform;
                }
                searchTime = Time.time + 0.5f;
            }
        }
        //Busca un objecto con TAG Player y si lo encuentra, lo setea al target de la camara al cargar la pantalla.
        private void SetCameraTarget()
        {
            Debug.Log("Buscando target player.");
            GameObject result = GameObject.FindGameObjectWithTag("Player");
            if (result != null)
            {
                Debug.Log("Target player encontrado. Seteando.");
                target = result.transform;
            }
            else
            {
                Debug.LogError("No se encontro Target Player.");
                return;
            }
        }
        
        //Busca el ultimo Tile con TAG y consigue las coordenadas para el clampeo.
        private void GetMaxClampX()
        {
            Debug.Log("Buscando ultimo Tile con TAG.");
            GameObject lastTile = GameObject.FindGameObjectWithTag("LastTile");
            if (lastTile != null)
            {
                Debug.Log("Se encontro ultimo Tile con TAG.");
                objMaxClampX = lastTile.transform;
                maxClampX = objMaxClampX.position.x;
            }
            else
            {
                Debug.LogError("No se encontro ultimo sprite con TAG");
                return;
            }              
        }

        //Busca el primer Tile con TAG y consigue las coordenadas para el clampeo
        private void GetMinClampX()
        {
            Debug.Log("Buscando primer Tile con TAG.");
            GameObject firstTile = GameObject.FindGameObjectWithTag("FirstTile");
            if (firstTile != null)
            {
                Debug.Log("Se encontro primer Tile con TAG.");
                objMinClampX = firstTile.transform;
                minClampX = objMinClampX.position.x;
                
            }
            else
            {
                Debug.LogError("No se encontro primer Tile con TAG");
                return;
            }
        }

        //Consige los pixeles de la esquina superior derecha de la camara.
        private void getRightCam()
        {
            derechaCam.x = Camera.main.pixelWidth;
            derechaCam.y = Camera.main.pixelHeight;
            derechaCam.z = 0f;
        }
        


    }
}