using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerMovementState
    {
        private GameObject bezierController;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            bezierController=GameObject.FindWithTag("RollerCoaster");
            bezierController.GetComponent<BezierController>().enabled=true;
        }
    }
}
