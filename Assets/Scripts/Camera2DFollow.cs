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
        public float minClamp = 0f;
        public float maxClamp = Mathf.Infinity;
        public float searchTime = 0f;

        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;
        
        private void Awake()
        {
            if (target == null){
                   SetCameraTarget();
            }            
        }


        // Use this for initialization
        private void Start()
        {
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }

        // Update is called once per frame
        private void Update()
        {

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

            newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, minClamp, maxClamp), newPos.z);
               
            transform.position = newPos;

            lastTargetPosition = target.position;
        }

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

    }
}