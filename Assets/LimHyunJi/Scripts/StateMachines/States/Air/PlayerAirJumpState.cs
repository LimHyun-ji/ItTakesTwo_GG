using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerAirJumpState : PlayerAirborneState
    {
        public PlayerAirJumpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void PhysicsUpdate()
        {
            base.Update();
        }

    }
}
