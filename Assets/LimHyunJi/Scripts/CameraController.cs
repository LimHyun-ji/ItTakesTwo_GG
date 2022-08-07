using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class CameraController : MonoBehaviour
    {
        public PlayerInput Input{get; private set;}
        public Transform CameraLookTransform;
        public float smoothSpeed=10f;
        public float mouseSpeed=10f;
        public float cameraObstacleSpeed=10f;
        public float minDistance=3f;
        public float baseDistance =20f;
        public float maxDistance =30f;
        private Vector3 initCamLocalPos;
        public Vector3 baseOffset;
        private Vector3 offset;
        private bool isObstacle;
        private Vector3 fixedPoint;
        float mouseX;
        float mouseY;
        

        private Transform target;
        private Vector3 velocity = Vector3.zero;
        private Vector2 cameraInput;
        private Vector2 playerMovementInput;

        // Start is called before the first frame update
        void Start()
        {
            initCamLocalPos=transform.localPosition;
            target = GameObject.FindWithTag("Player").transform;
            // Player player =target.GetComponent<Player>();
            // bool isnput=player.movementStateMachine.SwingState.isInput;
            // player.movementStateMachine.SwingState.
            Input = target.gameObject.GetComponent<PlayerInput>();
            offset = baseOffset;
            CameraLookTransform.position=target.position + offset;
        }
        private void Update() 
        {
            cameraInput = Input.PlayerActions.Look.ReadValue<Vector2>();
            playerMovementInput =Input.PlayerActions.Movement.ReadValue<Vector2>();
        }
        void LateUpdate()
        {
            Look();
            isObstacle=CheckIsObstacle();
        }

        void FixedUpdate()
        {
            if(!target) return;
            
            transform.forward=(CameraLookTransform.position- transform.position).normalized;
            FollowTarget(target);

            if(isObstacle)
            {
                //CameraLookTransform.position= target.position;
                transform.position = SetCameraPosition(transform.position, fixedPoint, baseOffset);
            }
            else
            {
                transform.localPosition = SetCameraPosition(transform.localPosition, initCamLocalPos, baseOffset);
            }
            
        }

        private void FollowTarget(Transform target)
        {
            Vector3 desiredPosition= target.position+offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(CameraLookTransform.position, desiredPosition, ref velocity, smoothSpeed);
            CameraLookTransform.position=smoothedPosition;
        }
        
        private void Look()
        {            
            mouseX += UnityEngine.Input.GetAxis("Mouse X")*mouseSpeed;
            mouseY += UnityEngine.Input.GetAxis("Mouse Y")*mouseSpeed;
            mouseY=Mathf.Clamp(mouseY, -60f, 60f);

            CameraLookTransform.eulerAngles =new Vector3(-mouseY, mouseX, 0);
            
        }



        private bool CheckIsObstacle()
        {
            //player 가 카메라 방향으로 ray를 쏜다
            Vector3 rayDir=(transform.position-CameraLookTransform.position).normalized;
            RaycastHit hitInfo;
            
            int layerMask = ((1 << LayerMask.NameToLayer("Player")));// | (1 << LayerMask.NameToLayer("Interactable")));  // Everything에서 Player,GUN 레이어만 제외하고 충돌 체크함
            layerMask  = ~layerMask ;
            //레이어 마스크로 사용하도록 코드 수정할 것
            if(Physics.Raycast(CameraLookTransform.position, rayDir, out hitInfo, 20f))
            {
                //Debug.Log(hitInfo.transform.gameObject.tag);
                //ray가 닿은 경우
                if(!((hitInfo.transform.gameObject.tag == "Player") || hitInfo.transform.gameObject.tag== "Hook"))//장애물에 닿은 경우
                {
                    fixedPoint=hitInfo.point;
                    return true;
                }
                //카메라 또는 player에 닿은 경우
                else
                    return false;
            }
            //ray 가 안 닿은 경우
            else
            {
                return false;
            }
        }

        private Vector3 SetCameraPosition(Vector3 currentPos, Vector3 cameraPos, Vector3 targetOffset)
        {
            offset=targetOffset;
            currentPos= Vector3.Lerp(currentPos, cameraPos, Time.deltaTime);
            float distance=Vector3.Distance(currentPos, cameraPos);
            if(distance<0.1f)
            {
                currentPos = cameraPos;
            }

            return currentPos;

        }

        private void SetBaseCameraDistance(float targetPointZ)
        {
            offset = baseOffset;
            Vector3 newCameraDistancePos=transform.localPosition;
            newCameraDistancePos.z -= cameraObstacleSpeed*Time.deltaTime;
            if(newCameraDistancePos.z<baseDistance)
                newCameraDistancePos.z=baseDistance;
            transform.localPosition=newCameraDistancePos;
        }

        private void SetCloseCamera()
        {
            offset = Vector3.zero;
            Vector3 newCameraDistancePos =transform.localPosition;
            newCameraDistancePos.z += cameraObstacleSpeed*Time.deltaTime;
            if(newCameraDistancePos.z > minDistance)
                newCameraDistancePos.z= minDistance;
            transform.localPosition=newCameraDistancePos;
        }

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
