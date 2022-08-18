using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerWallJumpingState : PlayerWallState
    {
        public PlayerWallJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter(); 
            stateMachine.Player.velocity.y=-1f;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UseGravity(20f);

            OnWallJump(wallData.wallJumpCount);

            bool isWall=WallCheck();
            if(!stateMachine.ReusableData.isGrounded && isWall)
            {
                stateMachine.ChangeState(stateMachine.WallIdleState);
            }

        }
        public override void Exit()
        {
            wallData.wallJumpCount++;
            UseGravity(20f);
            base.Exit();
        }
        protected override void AddInputActionsCallBacks()
        {
            
        }
        protected override void RemoveInputActionsCallBacks()
        {
            
        }

        void OnWallJump(int wallJumpCount)
        {
            Vector3 dir;
            if(wallData.wallJumpCount %2 ==0)
            {
                dir=wallData.WallJumpDir2;
            }
            else
            {
                dir=wallData.WallJumpDir1;
            }
            
            //부드럽게 돌기(추가할 사항)
            stateMachine.Player.transform.forward=dir;

            stateMachine.Player.transform.eulerAngles=new Vector3(0, stateMachine.Player.transform.eulerAngles.y, stateMachine.Player.transform.eulerAngles.z);

            dir+=Vector3.up;
            dir.Normalize();
            stateMachine.Player.characterController.Move(dir*12f*Time.deltaTime);
        }
        //앞 방향으로 레이 쏴서 벽(Ground)가 있는지 확인하기
        protected bool WallCheck()
        {
            Ray ray=new Ray(stateMachine.Player.transform.position, stateMachine.Player.transform.forward);
            RaycastHit hitInfo=new RaycastHit();
            int layerMask=(1 << LayerMask.NameToLayer("Wall"));

            if(Physics.SphereCast(stateMachine.Player.transform.position,0.5f,stateMachine.Player.transform.forward,out hitInfo, 0.1f, layerMask))
            {
                return true;
            }
            else return false;
        }

    }
}
