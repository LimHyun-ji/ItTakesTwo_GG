using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerBaseState
    {
        private GameObject bezierObj;
        BezierController bezierController;

        float value=0;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.velocity.y=-1f;
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();
            camera.currentState=CameraController.CameraState.RidingState;

            movementData.JumpData.airJumpCount=0;

            bezierController = stateMachine.Player.interactableObject.GetComponent<BezierController>();
            bezierController.enabled=true;

            bezierController.player = stateMachine.Player.gameObject;
            
            stateMachine.Player.interactableObject=null;
        }
        public override void PhysicsUpdate()
        {
            value += Time.deltaTime /2f;
            if(value>1)
            {
                OnFall();
            }

            stateMachine.Player.transform.position = bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value);
             //bezierController.BezierTest(bezierController.points, value, 3);
            stateMachine.Player.transform.forward= (bezierController.BezierTest(bezierController.points, value+Time.deltaTime, 3)- bezierController.BezierTest(bezierController.points, value, 3)).normalized;
            //(bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value+Time.deltaTime)- bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value)).normalized
            if(Vector3.Distance(stateMachine.Player.transform.position, bezierController.p4)<0.1)
            {
                //bezierController.enabled=false;
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
