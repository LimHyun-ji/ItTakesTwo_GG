using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            movementData.JumpData.airJumpCount=0;
            movementData.DashData.airDashCount=0;
            stateMachine.ReusableData.SpeedModifier =0f;
        }
        public override void Update()
        {
            base.Update();

            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
                return;
            OnMove();
        }
        #endregion
    }
}

