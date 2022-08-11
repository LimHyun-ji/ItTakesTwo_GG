using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerDashingState : PlayerMovingState
    {
        private PlayerDashData dashData;
        private float currentDashTime=0;
        float downForce;
        
        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData=movementData.DashData;
        }
        #region  IState Methods
        public override void Enter()
        {
            base.Enter();    
            stateMachine.Player.isMovable=false;   
            if(!stateMachine.ReusableData.isGrounded)   
            {
                movementData.DashData.airDashCount++;
                downForce=0f;
            }
            else
                downForce=movementData.SlopeData.SlopeForce*100;
                
            stateMachine.ReusableData.SpeedModifier=dashData.speedModifier;

        }
        public override void PhysicsUpdate()
        {
            //대시는 다른 방식으로 수정할 것
            base.PhysicsUpdate();
            
            Debug.Log(stateMachine.Player.characterController.velocity.y);
            currentDashTime +=Time.deltaTime;
            
            stateMachine.Player.characterController.Move(Time.deltaTime * stateMachine.Player.transform.forward* GetMovementSpeed() + Vector3.down * downForce*Time.deltaTime);
            if(currentDashTime > movementData.DashData.DashTime)
            {
                if(stateMachine.ReusableData.isGrounded)
                    stateMachine.ChangeState(stateMachine.IdlingState);
                else
                    stateMachine.ChangeState(stateMachine.FallingState);
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
