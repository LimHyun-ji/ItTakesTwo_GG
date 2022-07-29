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
            stateMachine.Player.velocity=Vector3.zero;
            if(!isGrounded)
            {
                movementData.JumpData.airJumpCount++;
            }
            
            stateMachine.Player.velocity.y += Mathf.Sqrt(jumpHeight * -2f *-9.8f);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            OnFall();
        }
        public override void Exit()
        {
            base.Exit();
        }

    }
}
