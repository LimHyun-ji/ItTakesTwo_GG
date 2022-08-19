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
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UseGravity(45f);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.animator.SetBool("IsFalling", false);
        }
    }
}
