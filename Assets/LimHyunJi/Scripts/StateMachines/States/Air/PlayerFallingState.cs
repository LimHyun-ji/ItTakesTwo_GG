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
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UseGravity(45f);
        }
    }
}
