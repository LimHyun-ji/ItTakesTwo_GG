using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerJumpingState : PlayerMovingState
    {
        private float jumpHeight;
        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            jumpHeight=movementData.JumpData.JumpHeight;
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetTrigger("Jump");
            stateMachine.Player.dummyAnimator.SetTrigger("Jump");

            stateMachine.Player.velocity.y=0f;            
            if(!stateMachine.ReusableData.isGrounded)
            {
                movementData.JumpData.airJumpCount++;
            }
            stateMachine.Player.velocity.y += Mathf.Sqrt(jumpHeight * -2f *-9.8f);

            if(stateMachine.Player.isJumppedPad)
            {
                stateMachine.Player.velocity.y -= Mathf.Sqrt(jumpHeight * -2f *-9.8f);
                camera.currentState =CameraController.CameraState.MagnetJumpPadState;//거리 조정
                camera.Play();//Shake
            }

            //jumpPad 아닐 때만
            
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            OnFall();
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.animator.ResetTrigger("Jump");
            stateMachine.Player.dummyAnimator.ResetTrigger("Jump");    

            camera.currentState =CameraController.CameraState.IdleState;//거리 조정
            
        }

    }
}
