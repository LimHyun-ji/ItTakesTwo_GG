using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;
        private float currentDashTime=0;
        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData=movementData.DashData;
        }
        #region  IState Methods
        public override void Enter()
        {
            base.Enter();    
            stateMachine.Player.isMovable=false;        
            stateMachine.ReusableData.SpeedModifier=dashData.speedModifier;
        }
        public override void PhysicsUpdate()
        {
            currentDashTime +=Time.deltaTime;
            stateMachine.Player.transform.position += stateMachine.Player.transform.forward* GetMovementSpeed() * Time.deltaTime;
            if(currentDashTime > movementData.DashData.DashTime)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                currentDashTime=0;
            }            
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.isMovable=true;        

        }
        #endregion
   }
}
