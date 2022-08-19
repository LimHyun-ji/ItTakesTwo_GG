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
            stateMachine.ReusableData.SpeedModifier=sprintData.speedModifier;
        }
        public override void Exit()
        {
            stateMachine.Player.animator.SetBool("IsSprinting", false);
        }
        
        #region Reusable Methods
        
        #endregion
    }
}
