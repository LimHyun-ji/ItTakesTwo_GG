using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerAirborneState : PlayerMovementState
    {
        protected bool canFly;
        public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
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
        public override void OnTriggerEnter(Collider other) 
        {
            if(( ((1 << other.gameObject.layer) & stateMachine.Player.GroundLayers) != 0))
            {
                OnLand();
            }
        }

        private void OnLand()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.DownForce.performed += OnDownForce;
        }
        protected override void RemoveinputActionsCallBacks()
        {
            base.RemoveinputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.DownForce.performed -= OnDownForce;
        }
        
        public void OnDownForce(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.ForceDownState);
        }
        #endregion
    }
}
