using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerGroundedState : PlayerMovementState
    {
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
            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
            stateMachine.Player.Input.PlayerActions.Sprint.performed +=OnSprintPerformed;
            stateMachine.Player.Input.PlayerActions.Jump.started += OnJump;

        }

        protected override void RemoveinputActionsCallBacks()
        {
            base.RemoveinputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
            stateMachine.Player.Input.PlayerActions.Sprint.performed -=OnSprintPerformed;
            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJump;

        }

        protected virtual void OnMove()
        {
            stateMachine.ChangeState(stateMachine.RunningState);   
        }
        

        public override void OnTriggerExit(Collider other) 
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                bool grounded = CheckGroundLayers();
                if(grounded)
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
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.DashingState);
        }
        public void OnSprintPerformed(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.JumpingState);
        }

        protected void OnFall()
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
        #endregion
    }
}
