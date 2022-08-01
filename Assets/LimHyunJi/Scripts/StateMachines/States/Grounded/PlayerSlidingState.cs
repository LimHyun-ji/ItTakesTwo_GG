using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerSlidingState : PlayerGroundedState
    {
        public PlayerSlidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        Vector3 movementDir=Vector3.zero;
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedModifier=movementData.SlopeData.speedModifier;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        }

        public override void Exit()
        {
            base.Exit();
        }
        protected override void Move(Vector3 environmentDir)
        {
            movementDir=GetSlopeDirection();
            stateMachine.Player.characterController.Move(movementDir*GetMovementSpeed()*Time.deltaTime);
            //base.Move(movementDir);

        }
        
        protected override void AddInputActionsCallBacks()
        {
            stateMachine.Player.Input.PlayerActions.Slide.performed += OnExitSlide;
        }
        protected override void RemoveInputActionsCallBacks()
        {
            stateMachine.Player.Input.PlayerActions.Slide.performed -= OnExitSlide;
        }
        protected void OnExitSlide(InputAction.CallbackContext obj)
        {
            shouldSlide=false;
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        RaycastHit slopeHit;

        private Vector3 GetSlopeDirection()
        {
            
            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out slopeHit, stateMachine.Player.characterController.height/2* movementData.SlopeData.slopeForceRayLength,1<<8))
            {
                if(slopeHit.normal ==Vector3.up) 
                    return Vector3.zero;
                Vector3 slopeDir= Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);
                float slideSpeed=GetMovementSpeed()+Time.deltaTime;

                movementDir = slopeDir * -slideSpeed;
                movementDir.y=movementDir.y -movementData.SlopeData.SlopeForce*100*Time.deltaTime;
                if(movementDir.y>0) movementDir.y=0f;
                return movementDir;
                //stateMachine.Player.characterController.Move(movementDir*Time.deltaTime);
            }
            return Vector3.zero;

        }
        

        
    }
}
