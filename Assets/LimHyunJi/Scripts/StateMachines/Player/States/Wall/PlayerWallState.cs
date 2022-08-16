using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerWallState : PlayerBaseState
    {
        protected PlayerWallData wallData;
        float wallJumpHeight=1.5f;
        List<GameObject> walls= new List<GameObject>();
        CameraController camera;

        public PlayerWallState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            wallData=stateMachine.Player.Data.wallData;
        }
        public override void Enter()
        {
            base.Enter();
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();
            camera.currentState=CameraController.CameraState.WallState;

            UseGravity(9.8f);
            
            if(wallData.wallJumpCount!=0)
            {

                //효율적이지 않음 ㅠㅠ
                // GameObject wall = GetNearestWall();
                // wallData.WallJumpDir1=GetJumpDirection1(wall);//첫번째 점프는 가까운 벽 아래 일정 지점으로 보내기
                // wallData.WallJumpDir2=GetJumpDirection2(wallData.WallJumpDir1);
            }

            if(wallData.wallJumpCount==1)
            {
                //camera.CameraLookTransform.forward= -stateMachine.Player.transform.right;

                GameObject wall = GetNearestWall();
                wallData.WallJumpDir1= -GetDirection();
                wallData.WallJumpDir2 = GetDirection();
            }            
        }
        public override void Exit()
        {
            base.Exit();
            camera.currentState=CameraController.CameraState.IdleState;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            UseGravity(20f);

            if(((1 << collider.gameObject.layer) & stateMachine.Player.GroundLayers) != 0)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
            
        
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            UseGravity(9.8f);

        }

        //벽에 닿았는지 체크
        protected bool WallAreaCheck(Collider collider)
        {
            if(1<<collider.gameObject.layer == LayerMask.GetMask("WallArea"))
            {
                return true;
            }
            else 
                return false;
        }
        protected Vector3 GetDirection()
        {
            Vector3 dir= walls[1].transform.position-walls[0].transform.position;
            dir.Normalize();
            return dir;
        }

        protected Vector3 GetJumpDirection1(GameObject target)
        {
            Vector3 dir=Vector3.zero;
            //if(wall[1])
            //dir= (wall[0].transform.position-wall[1].transform.position).normalized;
            dir= (target.transform.position
                    + new Vector3(0,target.transform.position.y+ wallJumpHeight*wallData.wallJumpCount, 0)) 
                    - stateMachine.Player.transform.position;
            return dir.normalized;
        }
        //Jump1의 반사벡터 구하기
        protected Vector3 GetJumpDirection2(Vector3 jumpDir1)
        {
           Ray ray= new Ray(stateMachine.Player.transform.position, jumpDir1);
           RaycastHit hitInfo;
           Vector3 normalVector;
           Vector3 reflectvector=Vector3.zero;

           int layerMask= (1 << LayerMask.NameToLayer("Wall"));
           //충돌한 면의 법선벡터 구하기
           if(Physics.Raycast(ray, out hitInfo, 10f, layerMask))
           {
                normalVector=hitInfo.normal;
                reflectvector= Vector3.Reflect(jumpDir1, normalVector);
                reflectvector.Normalize();
           }
           return reflectvector;
        }


        protected GameObject GetNearestWall()
        {
            int layerMask=(1 << LayerMask.NameToLayer("Wall"));
            //10f이내의 Wall layer 오브젝트 찾기            
            Collider[] coll= Physics.OverlapSphere(stateMachine.Player.transform.position, 10f, layerMask);
            float minDistance=10f;
            float secondMin=10f;
            GameObject targetWallObj=null;
            
            foreach (Collider obj in coll)
            {
                walls.Add(obj.gameObject);
                
                Debug.Log("Wall"+obj.gameObject);
                float distance = Vector3.Distance(stateMachine.Player.transform.position, obj.gameObject.transform.position);
    
                if (distance < minDistance) // 위에서 잡은 기준으로 거리 재기
                {
                    //가장 가까운 오브젝트
                    targetWallObj=obj.gameObject;
                    //secondMin=minDistance;
                    minDistance = distance;
                }
                // else if(distance<secondMin && distance != minDistance)
                // {
                //     //두번째로 가까운 오브젝트
                //     secondMin=distance;
                //     targetWallObj[1]=obj.gameObject;
                // }
            }
            return targetWallObj;
        }

        protected void OnLand()
        {
            stateMachine.ReusableData.isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers();

            Debug.Log("isGrounded"+stateMachine.ReusableData.isGrounded);
            
            {
            }
        }
    }
}
