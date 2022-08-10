using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerWallJumpingState : PlayerBaseState
    {
        bool isWall;
        public PlayerWallJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
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

        //벽이랑 닿으면 벽 Idle
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            // if(isWall)
            //     stateMachine.ChangeState();// WallIdle

        }
        private bool WallCheck(Collider collider)
        {
            if(1<<collider.gameObject.layer == LayerMask.GetMask("Wall"))
            {
                return true;
            }
            else 
                return false;
        }






        
    }
}
