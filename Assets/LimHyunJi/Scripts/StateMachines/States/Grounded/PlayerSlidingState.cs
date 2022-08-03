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
        RaycastHit slopeHit;
        float slideForce=2f;
        float slideSpeed;
        public override void Enter()
        {
            base.Enter();
            isInput=false;
            stateMachine.ReusableData.SpeedModifier=movementData.SlopeData.speedModifier;
            slideSpeed=5*2;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            SetPlayerRotation(slopeHit.normal);
        }

        public override void Exit()
        {
            movementDir=Vector3.zero;
            SetPlayerRotation(Vector3.up);
            base.Exit();
        }
        protected override void Move(Vector3 environmentDir, float environmentForce)
        {
            movementDir=GetSlopeDirection();
            //stateMachine.Player.characterController.Move(movementDir*slideSpeed*Time.deltaTime);
            base.Move(movementDir,slideSpeed);

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
        private Vector3 GetSlopeDirection()
        {
            
            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out slopeHit, stateMachine.Player.characterController.height/2* movementData.SlopeData.slopeForceRayLength,1<<8))
            {
                if(slopeHit.normal ==Vector3.up)
                {
                    slideSpeed = 0f;// Mathf.Lerp(slideSpeed , 0f, Time.deltaTime);
                    return movementDir;//이전 값 리턴

                }
                Vector3 slopeDir= Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);

                movementDir = -slopeDir * slideSpeed;//* -slideSpeed;
                movementDir.y=movementDir.y -movementData.SlopeData.SlopeForce*100*Time.deltaTime;
                if(movementDir.y>0) movementDir.y=0f;
                return movementDir;
            }
            return Vector3.zero;
        }        

    
        //나중에 수정할 것
        private void SetPlayerRotation(Vector3 dir)
        {

            // float x= Mathf.Lerp(stateMachine.Player.transform.up.x, dir.x, Time.deltaTime);
            // float y= Mathf.Lerp(stateMachine.Player.transform.up.x, dir.y, Time.deltaTime);
            // float z= Mathf.Lerp(stateMachine.Player.transform.up.x, dir.z, Time.deltaTime);

            //stateMachine.Player.transform.up=new Vector3(x, y, z);

            stateMachine.Player.transform.up= dir;
            
        }
    }
}
