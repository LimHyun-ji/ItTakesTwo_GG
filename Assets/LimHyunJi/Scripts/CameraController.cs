using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class CameraController : MonoBehaviour
    {
        public float followSpeed=10f;
        public float smoothTime=1f;
        public Vector3 offset;
        public Transform CameraLookTransform;

        public PlayerInput Input{get; private set;}
        private Transform target;
        //private Vector3 velocity = Vector3.zero;
        //private float mx;
        //private float my;
        private Vector2 cameraInput;
        private Vector2 playerMovementInput;
        //private Vector3 cameraEulerAngle;
        private Vector3 desiredPosition;
        Vector3 smoothedPosition;
        Vector3 smoothedRotation;
        
        public float distance=3f;
        public float speed=2f;
        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            //CameraLookTransform=target.transform;
            
            Input = target.gameObject.GetComponent<PlayerInput>();

            

            //LookAt
            transform.forward =(target.position - transform.position).normalized;
            //transform.eulerAngles=new Vector3(transform.eulerAngles.x, 0, 0);

            desiredPosition= target.position + offset;
            transform.position=desiredPosition;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            cameraInput = Input.PlayerActions.Look.ReadValue<Vector2>();
            playerMovementInput =Input.PlayerActions.Movement.ReadValue<Vector2>();
            
            CameraLookTransform.position=target.position;
            if(target)
            {
                
               //FollowTarget(target);
               Look();
            }
        }

         private void FollowTarget(Transform target)
        {
        //     //좌우 할때는 약간 돌아감
            
             desiredPosition= target.position + offset;
        //     //smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            
        //     if(Vector3.Distance( desiredPosition, transform.position ) >0)
        //     {
        //         Vector3 moveDir= (desiredPosition -transform.position).normalized;
        //         transform.position += moveDir * 1f*Time.deltaTime;
        //     }
            
        //     // transform.forward =(target.position - transform.position).normalized;
        //     // transform.eulerAngles=new Vector3(transform.eulerAngles.x, 0, 0);
                transform.position=desiredPosition;
         }

         private void Look()
         {
            Vector3 eulerAngle= CameraLookTransform.eulerAngles;
            eulerAngle.y += (cameraInput.x*Time.deltaTime);
            eulerAngle.x +=(cameraInput.y*Time.deltaTime);

            CameraLookTransform.eulerAngles=eulerAngle;
         }
        // private void Look()
        // {
        //     //transform.RotateAround(target.position, Vector3.up, cameraInput.x*Time.deltaTime*10f);
            
            
        //     //smoothedPosition = Vector3.SmoothDamp(transform.position, cameraTransform.position, ref velocity, smoothSpeed);
        //     //smoothedRotation=Vector3.SmoothDamp(transform.eulerAngles, cameraTransform.eulerAngles, ref velocity, smoothSpeed);
            
        //     //transform.position =smoothedPosition;
        //     //transform.eulerAngles=smoothedRotation;


        //     Vector3 moveDir = (target.position - transform.position).normalized;
        //     Vector3 normal = Vector3.Cross(moveDir, Vector3.up);
        //     // Debug.Log(cameraInput.x);
        //     desiredPosition += normal.normalized * cameraInput.x *Time.deltaTime ;
        //     //transform.position=desiredPosition;
        //     if(Vector3.Distance( desiredPosition, target.position ) > -offset.z)
        //     {
        //         transform.position += moveDir *smoothSpeed*Time.deltaTime;
        //     }
        // }


        // Debug.Log(GetTargetDistance());
        //         if(GetTargetDistance()> Mathf.Abs(offset.z+1f))
        //         {
        //             transform.position += (GetFollowDirection()*followSpeed)*Time.deltaTime;
        //         }
        //         if(GetTargetDistance()<Mathf.Abs(offset.z-1f))
        //         {
        //             transform.position -= GetFollowDirection()*followSpeed*Time.deltaTime;
        //         }

        //         transform.position += GetRotateDirection()*cameraInput.x*Time.deltaTime;
        //         //LookAt
        //         //Vector3 lookDir =(target.position - transform.position).normalized;
        //         //transform.forward=lookDir;
        
        private Vector3 GetFollowDirection()
        {
            Vector3 moveDir = (target.position-transform.position).normalized;

            return new Vector3(moveDir.x, 0, moveDir.z);
        }
        private Vector3 GetRotateDirection()
        {
            Vector3 moveDir = (target.position - transform.position).normalized;
            Vector3 normal = Vector3.Cross(moveDir, Vector3.up);

            return normal;
        }
        private float GetTargetDistance()
        {
            return Vector3.Distance(target.position, transform.position);
        }
    }

}
