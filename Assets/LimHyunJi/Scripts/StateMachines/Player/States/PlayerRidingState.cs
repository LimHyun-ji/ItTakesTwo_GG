using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerBaseState
    {
        private GameObject bezierObj;
        BezierController bezierController;
        CameraController camera;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.interactableObject=null;
            stateMachine.Player.velocity.y=-1f;
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();
            camera.currentState=CameraController.CameraState.RidingState;

            movementData.JumpData.airJumpCount=0;
            bezierObj=GameObject.FindWithTag("RollerCoaster");
            bezierController = bezierObj.GetComponent<BezierController>();
            bezierController.enabled=true;
            bezierController.value=0f;

            bezierController.player = stateMachine.Player.gameObject;
        }
        public override void PhysicsUpdate()
        {
            if(Vector3.Distance(stateMachine.Player.transform.position, bezierController.P4)<0.1)
            {
                bezierController.enabled=false;
                OnFall();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            bezierController.enabled=false;
            //stateMachine.Player.characterController.Move(stateMachine.Player.transform.forward * 3f);
            camera.currentState=CameraController.CameraState.IdleState;
        }
        
    }
}
