using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerWallIdleState : PlayerWallState
    {
        float currentTime;
        public PlayerWallIdleState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            currentTime=0f;

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            currentTime+= Time.deltaTime;
            if(currentTime>wallData.wallIdleTime)
            {
                UseGravity(0.0001f);
            }
        }
        
        
    }
}
