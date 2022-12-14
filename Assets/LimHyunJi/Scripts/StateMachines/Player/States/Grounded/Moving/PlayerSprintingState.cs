using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerDashData sprintData;//재사용
        private bool keepSprinting;
        private float currentSprintTime;
        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData=movementData.DashData;
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetBool("IsSprinting", true);
            stateMachine.Player.dummyAnimator.SetBool("IsSprinting", true);

            stateMachine.ReusableData.SpeedModifier=sprintData.speedModifier;
            AudioPlay(movementData.walkSound, true, 0.4f);

        }
        public override void Exit()
        {
            stateMachine.Player.animator.SetBool("IsSprinting", false);
            stateMachine.Player.dummyAnimator.SetBool("IsSprinting", false);

            stateMachine.Player.audioSource.Stop();

        }
        
        #region Reusable Methods
        
        #endregion
    }
}
