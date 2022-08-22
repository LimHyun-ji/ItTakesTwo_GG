using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerFallingState : PlayerAirborneState
    {
        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetBool("IsFalling", true);
            stateMachine.ReusableData.SpeedModifier= 1f;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UseGravity(45f);
            if(stateMachine.ReusableData.isGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.animator.SetBool("IsFalling", false);
        }
    }
}
