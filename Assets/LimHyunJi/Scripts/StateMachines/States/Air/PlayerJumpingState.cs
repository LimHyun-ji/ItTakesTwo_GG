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
            //currentDelayTime=0f;
            //isGrounded= CheckGroundLayers();
            //if(isGrounded)
            stateMachine.Player.rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        public override void Update()
        {
            base.Update();
            // currentDelayTime +=Time.deltaTime;
            // if(isGrounded && currentDelayTime>delayTime)
            // {
            //     stateMachine.ChangeState(stateMachine.IdlingState);
            // }
            OnFall();
        }

    }
}
