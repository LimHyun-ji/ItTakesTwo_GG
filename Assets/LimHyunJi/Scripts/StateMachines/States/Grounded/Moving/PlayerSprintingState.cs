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
            stateMachine.ReusableData.SpeedModifier=sprintData.speedModifier;
            //startTime =Time.time;
            currentSprintTime=0f;
        }
        public override void Update()
        {
            base.Update();
            currentSprintTime += Time.deltaTime;
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
        #region Reusable Methods
        
        #endregion
    }
}
