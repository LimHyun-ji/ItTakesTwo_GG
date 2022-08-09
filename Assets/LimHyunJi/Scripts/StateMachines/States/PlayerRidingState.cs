using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerAirborneState
    {
        private GameObject bezierObj;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void PhysicsUpdate()
        {

        }

        public override void Enter()
        {
            bezierObj=GameObject.FindWithTag("RollerCoaster");
            BezierController bezierController = bezierObj.GetComponent<BezierController>();
            bezierController.enabled=true;
            bezierController.value=0f;
            bezierController.player = stateMachine.Player.gameObject;
        }
    }
}
