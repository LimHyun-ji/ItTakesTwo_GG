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
            stateMachine.Player.velocity.y=0f;
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
            base.Exit();
        }
        protected override void AddInputActionsCallBacks()
        {
            
        }
        protected override void RemoveInputActionsCallBacks()
        {
            
        }

        //벽이랑 닿으면 벽 Idle
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            

        }
        void OnWallJump(int wallJumpCount)
        {
            Vector3 dir;
            if(wallJumpCount%2==0)
                dir =Vector3.right;
            else
                dir=Vector3.left;
            stateMachine.Player.transform.forward=dir;
            dir+=Vector3.up;
            dir.Normalize();
            stateMachine.Player.characterController.Move(dir*10f*Time.deltaTime);
        }
        //앞 방향으로 레이 쏴서 벽(Ground)가 있는지 확인하기
        protected bool WallCheck()
        {
            Ray ray=new Ray(stateMachine.Player.transform.position, stateMachine.Player.transform.forward);
            RaycastHit hitInfo=new RaycastHit();
            int layerMask=(1 << LayerMask.NameToLayer("Ground"));

            if(Physics.SphereCast(stateMachine.Player.transform.position,0.5f,stateMachine.Player.transform.forward,out hitInfo, 0.1f, layerMask))
            {
                return true;
            }
            else return false;

        }






        
    }
}
