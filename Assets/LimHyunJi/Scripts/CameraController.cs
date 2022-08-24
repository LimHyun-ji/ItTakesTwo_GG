using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class CameraController : MonoBehaviour
    {
        public enum CameraState
        {
            IdleState,
            RidingState,
            WallState, 
            MagnetJumpPadState, 
            CineMachineState
        }
        public CameraState currentState;

        public Transform CameraLookTransform;
        public float smoothSpeed=0.3f;
        public float baseSmoothSpeed=0.3f;
        public float ridingSmoothSpeed=0.1f;
        public float mouseSpeed=10f;
        public float cameraObstacleSpeed=10f;
        public float minDistance=3f;
        public float baseDistance =20f;
        public float maxDistance =30f;
        public float amplitude =15f;
        public float shakePlayTime=1.0f;

        private Vector3 initCamLocalPos;
        public Vector3 baseOffset;
        private Vector3 offset;
        private bool isObstacle;
        private Vector3 fixedPoint;
        float mouseX;
        float mouseY;
        Vector2 mouseInput=Vector2.zero;       

        public enum CameraType{camera1, camera2};
        public CameraType cameraName;
        public Transform target;
        public Transform otherTarget;
        private Vector3 velocity = Vector3.zero;
        private int  wallStateCount;
        private int idleStateCount;
        private Vector3 desiredDir;
        private PlayerInput Input;
        private Player player;
        private Vector3 originPos;
        

        public GameObject otherDummy;
        //transform position z값을 -20으로 설정해두기
        //그래야 나중에 virtual camera쓸때 다시 원래값으로 돌아올 수 있음
        void Start()
        {            
            initCamLocalPos=transform.localPosition;
            initCamLocalPos.z=-baseDistance;
            offset = baseOffset;
            CameraLookTransform.position=target.position + offset;

            currentState=CameraState.IdleState;
            Input=target.GetComponent<PlayerInput>();
            player= target.GetComponent<Player>();

            otherDummy.SetActive(false);
            
        }
        private void Update() 
        {
            // 카메라에서 상대방에게 Ray 쏘기
            FollowDummy();
            ActiveSilhouette();
        }
        void LateUpdate()
        {
            if(currentState ==CameraState.IdleState || currentState == CameraState.RidingState)
                Look();
        }

        //cineMachine State일 때는 cinemachine brain이 제어해서 상관없음
        void FixedUpdate()
        {
            if(!target) return;
            FollowTarget(target);
            transform.forward=(CameraLookTransform.position- transform.position).normalized;

            switch(currentState)
            {
                case CameraState.IdleState:
                    IdleFixedUpdate();
                    break;
                case CameraState.RidingState:
                    RidingFixedUpdate();
                    break;
                case CameraState.WallState:
                    WallFixedUpdate();
                    break;
                case CameraState.MagnetJumpPadState:
                    MagnetFixedUpdate();
                    break;
            }
        }


        protected void IdleFixedUpdate()
        {
            idleStateCount++;
            wallStateCount=0;

            smoothSpeed=baseSmoothSpeed;

            isObstacle=CheckIsObstacle();
            
            if(isObstacle)
            {
                transform.position = SetCameraPosition(transform.position, fixedPoint, baseOffset);
            }
            else
            {
                transform.localPosition = SetCameraPosition(transform.localPosition, initCamLocalPos, baseOffset);
            }
        }
        protected void RidingFixedUpdate()
        {
            smoothSpeed=ridingSmoothSpeed;
        }
        protected void WallFixedUpdate()
        {
            idleStateCount=0;
            wallStateCount++;
            if(wallStateCount==1)
            {
                desiredDir= player.cameraDir;
            }
            CameraLookTransform.forward= SetCameraRotation(CameraLookTransform.forward, desiredDir);
            
            //CameraLookTransform.forward= SetCameraRotation(CameraLookTransform.transform.forward, target.transform.forward);

            smoothSpeed=baseSmoothSpeed;
            Vector3 wallCamPos= new Vector3(0, 0, -maxDistance);

            transform.localPosition = SetCameraPosition(transform.localPosition, wallCamPos, baseOffset);

            //CameraLookTransform.forward=target.right;
            //측면 보기
            //CameraLookTransform.forward=CameraLookTransform.right;
            //transform.forward=CameraLookTransform.forward;
        }
        protected void MagnetFixedUpdate()
        {
            //자석패드에 붙을 때 Shake
            smoothSpeed=0f;
            Vector3 nearCamPos =new Vector3(0, 0, -minDistance);
            transform.localPosition = SetCameraPosition(transform.localPosition, nearCamPos, Vector3.zero);
        }


        //타겟 부드럽게 따라다니기
        private void FollowTarget(Transform target)
        {
            Vector3 desiredPosition= target.position+offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(CameraLookTransform.position, desiredPosition, ref velocity, smoothSpeed);
            CameraLookTransform.position=smoothedPosition;
        }
        
        //마우스돌려서 시점 바꾸기
        private void Look()
        {   
            if(wallStateCount>0)//초기화
            {
                SetMouseRotationInit(CameraLookTransform);
            }  
            //mouse 값 받기
            if(player.playerName==Player.PlayerType.Player1)
                mouseInput= Input.Player1Actions.Look.ReadValue<Vector2>();
            else if(player.playerName==Player.PlayerType.Player2)
                mouseInput= Input.Player2Actions.Look.ReadValue<Vector2>();


            mouseX += mouseInput.x*mouseSpeed*Time.deltaTime;// UnityEngine.Input.GetAxis("Mouse X")*mouseSpeed;
            mouseY +=mouseInput.y*mouseSpeed*Time.deltaTime;// UnityEngine.Input.GetAxis("Mouse Y")*mouseSpeed;
            mouseY=Mathf.Clamp(mouseY, -60f, 60f);

            CameraLookTransform.eulerAngles =new Vector3(-mouseY, mouseX, 0);
            
        }



        //장애물 있는지 판별
        //baseOffset값을 바꿔주고 싶은데 그렇게 하면 오류가 발생(쏘는 ray가 바뀌어서 왔다갔다함)
        private bool CheckIsObstacle()
        {
            //player 가 카메라 방향으로 ray를 쏜다
            Vector3 rayDir=(transform.position-CameraLookTransform.position).normalized;
            RaycastHit hitInfo;
            
            int layerMask = ((1 << LayerMask.NameToLayer("Ground")));// | (1 << LayerMask.NameToLayer("Interactable")));  // Everything에서 Player,GUN 레이어만 제외하고 충돌 체크함
            //layerMask  = ~layerMask ;
            //레이어 마스크로 사용하도록 코드 수정할 것
            if(Physics.Raycast(CameraLookTransform.position, rayDir, out hitInfo, 20f, layerMask))
            {
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
        private Vector3 SetCameraRotation(Vector3 currRot, Vector3 targetRot)
        {
            currRot=Vector3.Lerp(currRot, targetRot, Time.deltaTime);
            //float distance=Vector3.Distance(currRot, targetRot);

            return currRot;
        }
        public void SetMouseRotationInit(Transform transform)
        {
            float x=transform.eulerAngles.x;
                float y=transform.eulerAngles.y;

                if(transform.eulerAngles.x>180)
                    x-=360;
                if(transform.eulerAngles.y>180)
                    y-=360;
                mouseX=y;
                mouseY= -x;
        }

        #region CameraShake
        private void InitPos(Vector3 originPos)
        {
            this.originPos=originPos;
        }

        private void ShakeCamera()
        {
            transform.position = originPos+ UnityEngine.Random.insideUnitSphere * amplitude;
        }
        private void ShakeStop()
        {
            transform.position=originPos;
        }

        public void Play()
        {
            InitPos(transform.position);
            StopAllCoroutines();
            StartCoroutine(IPlay());
        }
        private IEnumerator IPlay()
        {
            float currentTime=0f;

            while(currentTime<shakePlayTime)
            {
                ShakeCamera();
                currentTime+= Time.deltaTime;
                yield return null;
            }
            ShakeStop();
        }
        #endregion

        private void FollowDummy()
        {
            //다른 타겟을 따라다녀야 함
            otherDummy.transform.position=new Vector3(otherTarget.transform.position.x, otherTarget.transform.position.y-1, otherTarget.transform.position.z);
            otherDummy.transform.rotation=otherTarget.transform.rotation;
        }
        private void ActiveSilhouette()
        {
            Vector3 dir= otherTarget.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);//카메라에서 다른 플레이어를 향해 ray쏘기
            RaycastHit hitInfo;
            int layermask= LayerMask.GetMask("Player", "Magnet", "Dummy2", "Dummy1", "Interactable", "SavePoint");
            layermask = ~layermask;      

            float distance = Vector3.Distance(otherTarget.transform.position , transform.position);      

            //Player, Magent Dummy가 아니면 Active
            if(Physics.Raycast(ray, out hitInfo, distance, layermask))
            {
                //Debug.Log(hitInfo.transform.gameObject.layer);
                //Debug.Log(hitInfo.transform.gameObject);

                otherDummy.SetActive(true);     
            }
            //닿은게 Player나 magnet,  Dummy이면 , 혹은 안 닿았으면 비활성화       
            else
            {
                //플레이어인데 자기면 장애물이라고 판단?
                // if(hitInfo.transform.gameObject != target)
                //     otherDummy.SetActive(true); 
                otherDummy.SetActive(false);
            }

            Debug.DrawLine(transform.position, otherTarget.transform.position, Color.red);
        }
    }

}
