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
            if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                OnLand();
            }
        }

        private void OnLand()
        {
            //Land
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
        }
        protected override void RemoveinputActionsCallBacks()
        {
            base.RemoveinputActionsCallBacks();
        }
        #endregion
    }
}
