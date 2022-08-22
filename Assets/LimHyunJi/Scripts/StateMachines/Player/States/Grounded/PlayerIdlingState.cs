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
            stateMachine.Player.Data.WallData.wallJumpCount=0;
            movementData.JumpData.airJumpCount=0;
            movementData.DashData.airDashCount=0;
            stateMachine.ReusableData.SpeedModifier =0f;
        }
        public override void PhysicsUpdate()
        {
            //stateMachine.Player.velocity.y=-1f;//use gravity 에서 땅판정 잘 못해서 계속 음수되는 것 방지
            
            base.PhysicsUpdate();
            // if(shouldSlide)
            // {
            //     stateMachine.ChangeState(stateMachine.SlidingState);
            //     return;
            // }
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
                return; 
            OnMove();
        }
        #endregion
    }
}

