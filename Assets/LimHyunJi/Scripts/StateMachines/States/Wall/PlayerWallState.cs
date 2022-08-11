using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerWallState : PlayerBaseState
    {
        protected PlayerWallData wallData;
        public PlayerWallState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            wallData=stateMachine.Player.Data.wallData;
        }
        public override void Enter()
        {
            base.Enter();
            UseGravity(9.8f);
            
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            UseGravity(9.8f);
            stateMachine.ReusableData.isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers();

            Debug.Log("isGrounded"+stateMachine.ReusableData.isGrounded);
            if(stateMachine.ReusableData.isGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
            
        
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            UseGravity(9.8f);

        }

        //벽에 닿았는지 체크
        protected bool WallAreaCheck(Collider collider)
        {
            if(1<<collider.gameObject.layer == LayerMask.GetMask("WallArea"))
            {
                return true;
            }
            else 
                return false;
        }
    }
}
