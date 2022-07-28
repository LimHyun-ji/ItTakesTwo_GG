using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerJumpingState : PlayerMovingState
    {
        private float jumpHeight;
        private bool isGrounded;
        private float currentDelayTime;
        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            jumpHeight=movementData.JumpData.JumpHeight;
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.velocity.y += Mathf.Sqrt(jumpHeight * -2f * -9.8f);
        }
        public override void Update()
        {
            base.Update();
            OnFall();
        }

    }
}
