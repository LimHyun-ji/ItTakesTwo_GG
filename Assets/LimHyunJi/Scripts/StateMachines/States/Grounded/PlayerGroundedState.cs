using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerGroundedState : PlayerMovementState
    {
        protected bool shouldSprint;
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }

        

        
        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.SprintToggle.started +=OnSprintToggle;

        }

        protected override void RemoveinputActionsCallBacks()
        {
            base.RemoveinputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.SprintToggle.started -=OnSprintToggle;

        }

        protected virtual void OnMove()
        {
            if(shouldSprint)
                stateMachine.ChangeState(stateMachine.SprintingState);   
            else
                stateMachine.ChangeState(stateMachine.RunningState);   
        }
        

        public override void OnTriggerExit(Collider other) 
        {
            if(( ((1 << other.gameObject.layer) & stateMachine.Player.GroundLayers) != 0))
            {
                if(CheckGroundLayers())
                    return;
                else
                    OnFall();
            }
        }

        #endregion

        #region Input Methods


        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        
        public void OnSprintToggle(InputAction.CallbackContext context)
        {
            shouldSprint = !shouldSprint;
        }
       
        #endregion
    }
}
