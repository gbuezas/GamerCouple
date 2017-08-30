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
            coordsDerechaCam = Camera.main.ScreenToWorldPoint(derechaCam);
            coordsIzquierdaCam = Camera.main.ScreenToWorldPoint(izquierdaCam);
            offsetCamIzquierda = coordsIzquierdaCam.x - newPos.x;
            offsetCamDerecha = coordsDerechaCam.x - newPos.x;
            
             /*if ((coordsDerechaCam.x + 3) >= maxClampX)
             {
                 newPos = new Vector3(Mathf.Clamp(newPos.x, minClampX, newPos.x), Mathf.Clamp(newPos.y, minClampY, maxClampY), newPos.z);
             }
             else
             {
                 newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, minClampY, maxClampY), newPos.z);
             }*/

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
        
        private void GetMaxClampX()
        {
            Debug.Log("Buscando ultimo sprite con TAG.");
            GameObject lastSprite = GameObject.FindGameObjectWithTag("LastSprite");
            if (lastSprite != null)
            {
                Debug.Log("Se encontro ultimo sprite con TAG.");
                objMaxClampX = lastSprite.transform;
                maxClampX = objMaxClampX.position.x;
            }
            else
            {
                Debug.LogError("No se encontro ultimo sprite con TAG");
                return;
            }              
        }
        private void GetMinClampX()
        {
            Debug.Log("Buscando primer sprite con TAG.");
            GameObject firstSprite = GameObject.FindGameObjectWithTag("FirstSprite");
            if (firstSprite != null)
            {
                Debug.Log("Se encontro primer sprite con TAG.");
                objMinClampX = firstSprite.transform;
                minClampX = objMinClampX.position.x;
                
            }
            else
            {
                Debug.LogError("No se encontro primer sprite con TAG");
                return;
            }
        }
        private void getRightCam()
        {
            derechaCam.x = Camera.main.pixelWidth;
            derechaCam.y = Camera.main.pixelHeight;
            derechaCam.z = 0f;
        }
        


    }
}