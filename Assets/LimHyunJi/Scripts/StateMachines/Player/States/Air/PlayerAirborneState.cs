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
            stateMachine.Player.animator.SetBool("IsAir", true);
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void OnTriggerEnter(Collider collider) 
        {
            base.OnTriggerEnter(collider);
            if(((1 << collider.gameObject.layer) & stateMachine.Player.GroundLayers) != 0)
            {
                OnLand();
            }
            

        }
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            
        }

        private void OnLand()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.DownForce.performed += OnDownForce;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.DownForce.performed += OnDownForce;
            }


        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.DownForce.performed -= OnDownForce;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.DownForce.performed -= OnDownForce;
            }
        }

        

        public void OnDownForce(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.ForceDownState);
        }
        #endregion
    }
}
