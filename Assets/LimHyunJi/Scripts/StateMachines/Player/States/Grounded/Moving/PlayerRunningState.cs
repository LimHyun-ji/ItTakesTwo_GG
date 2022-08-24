using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerRunningState : PlayerMovingState
    {
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetBool("IsRunning", true);
            stateMachine.Player.dummyAnimator.SetBool("IsRunning", true);

            stateMachine.ReusableData.SpeedModifier=movementData.RunData.speedModifier;
        }
        #endregion

        #region Input Methods

        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.animator.SetBool("IsRunning", false);
            stateMachine.Player.dummyAnimator.SetBool("IsRunning", false);
        }
        #endregion
    }
}
