using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerMovementState
    {
        private GameObject bezierObj;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            bezierObj=GameObject.FindWithTag("RollerCoaster");
            BezierController bezierController = bezierObj.GetComponent<BezierController>();
            bezierController.enabled=true;
            bezierController.value=0f;
            bezierController.player = stateMachine.Player.gameObject;
        }
    }
}
