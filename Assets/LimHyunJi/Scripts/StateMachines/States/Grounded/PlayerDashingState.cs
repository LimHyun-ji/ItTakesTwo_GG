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
            stateMachine.ReusableData.SpeedModifier=dashData.speedModifier;
            AddForceOnTransitionFromStationaryState();
        }
        public override void Update() 
        {
            currentDashTime +=Time.deltaTime;
            if(currentDashTime > movementData.DashData.DashTime)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                currentDashTime=0;
            }            
        }
          public override void OnAnimationTransitionEvent()
        {
        }

        #endregion
        # region Main Methods
        private void AddForceOnTransitionFromStationaryState()
        {
            if(stateMachine.ReusableData.MovementInput != Vector2.zero)
                return;
            Vector3 characterRotationDir=stateMachine.Player.transform.forward;
            characterRotationDir.y=0f;//이거 왜 해주는지?

            stateMachine.Player.rigidBody.velocity=characterRotationDir * GetMovementSpeed();
        }

        #endregion

        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }

        #endregion
    }
}
