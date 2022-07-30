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
        protected bool shouldSlide;
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
            stateMachine.Player.Input.PlayerActions.Slide.started += OnSlide;
        }

        protected override void RemoveinputActionsCallBacks()
        {
            base.RemoveinputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.SprintToggle.started -=OnSprintToggle;
            stateMachine.Player.Input.PlayerActions.Slide.started -= OnSlide;

        }

        protected virtual void OnMove()
        {
            // if(shouldSlide)
            //     stateMachine.ChangeState(stateMachine.SlidingState);
            if(shouldSprint)
                stateMachine.ChangeState(stateMachine.SprintingState);   
            else
                stateMachine.ChangeState(stateMachine.RunningState);   
        }
        

        public override void OnTriggerExit(Collider other) 
        {
            if(( ((1 << other.gameObject.layer) & stateMachine.Player.GroundLayers) != 0))
            {
                if(stateMachine.Player.characterController.isGrounded || CheckGroundLayers())
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
        private void OnSlide(InputAction.CallbackContext obj)
        {
            shouldSlide = !shouldSlide;
        }
        #endregion
    }
}
