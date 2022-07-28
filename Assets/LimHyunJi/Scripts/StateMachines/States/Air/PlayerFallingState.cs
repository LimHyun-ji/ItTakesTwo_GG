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
        public override void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.gameObject.layer);
            if(other.gameObject.layer == 3)//Ground
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
        
    }
}
