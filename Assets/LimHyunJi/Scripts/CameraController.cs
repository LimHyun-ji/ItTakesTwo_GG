using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class CameraController : MonoBehaviour
    {
        public float smoothSpeed=0.3f;
        public float smoothTime=1f;
        public Vector3 offset;
        public Transform cameraTransform;

        public PlayerInput Input{get; private set;}
        private Transform target;
        private Vector3 velocity = Vector3.zero;
        private float mx;
        private float my;
        private Vector2 cameraInput;
        private Vector2 playerMovementInput;
        private Vector3 cameraEulerAngle;
        private Vector3 desiredPosition;
        Vector3 smoothedPosition;
        Vector3 smoothedRotation;
        
        public float distance=3f;
        public float speed=2f;
        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            Input = target.gameObject.GetComponent<PlayerInput>();

            desiredPosition= target.position + offset;
            transform.position=desiredPosition;
            //cameraTransform=transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            cameraInput = Input.PlayerActions.Look.ReadValue<Vector2>();
            playerMovementInput =Input.PlayerActions.Movement.ReadValue<Vector2>();

            if(target)
            {
                
                //FollowTarget(target);
                Look();  
                
                //if( playerMovementInput.x == 0f)
                    //transform.LookAt(target);

            }
        }

        private void FollowTarget(Transform target)
        {
            //좌우 할때는 약간 돌아감
            desiredPosition= target.position + offset;

            smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = desiredPosition;
        }
        private void Look()
        {
            cameraTransform.RotateAround(target.position, Vector3.up, cameraInput.x*Time.deltaTime*10f);
            smoothedPosition = Vector3.SmoothDamp(transform.position, cameraTransform.position, ref velocity, smoothSpeed);
            smoothedRotation=Vector3.SmoothDamp(transform.eulerAngles, cameraTransform.eulerAngles, ref velocity, smoothSpeed);
            
            //transform.position =smoothedPosition;
            //transform.eulerAngles=smoothedRotation;


            // Vector3 moveDir = (target.position - transform.position).normalized;
            // Vector3 normal = Vector3.Cross(moveDir, Vector3.up);
            // Debug.Log(cameraInput.x);
            // desiredPosition += normal.normalized * cameraInput.x *Time.deltaTime ;
        }

    }

}
