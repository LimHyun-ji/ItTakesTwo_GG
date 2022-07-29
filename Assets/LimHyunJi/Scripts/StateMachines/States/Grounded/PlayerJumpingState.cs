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
            bool grounded =CheckGroundLayers();
            if(!isGrounded)
            {
                movementData.JumpData.airJumpCount++;
            }
            
            //stateMachine.Player.velocity.y += Mathf.Sqrt(jumpHeight * -2f * -9.8f);
            stateMachine.Player.velocity.y += Mathf.Sqrt(jumpHeight * -2f *-9.8f);
            //stateMachine.Player.rigidBody.AddForce(Vector2.up * jumpHeight, ForceMode.Impulse);
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
