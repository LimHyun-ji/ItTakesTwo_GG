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
        Vector3 environmentDir=Vector3.zero;
        RaycastHit slopeHit;
        float slideForce=2f;
        float newEnvironmentForce;
        float addExternalSpeed;

        float slideSpeed;
        float moveSpeed;
        float currentTime=0f;
        public override void Enter()
        {
            base.Enter();
            isInput=false;
            stateMachine.ReusableData.SpeedModifier=movementData.SlopeData.speedModifier;
            slideSpeed=10f;
            stateMachine.Player.animator.SetBool("IsSliding", true);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            environmentDir=Vector3.zero;
            stateMachine.Player.animator.SetBool("IsSliding", false);
            ResetPlayerRotation();
            base.Exit();
        }
        protected override void Move()//재정의
        {
            environmentDir=GetSlopeDirection();
            Vector3 inputDir = stateMachine.Player.transform.right * stateMachine.ReusableData.MovementInput.x
                                + stateMachine.Player.transform.forward * stateMachine.ReusableData.MovementInput.y;
            newEnvironmentForce = Mathf.Lerp(newEnvironmentForce, slideSpeed, Time.deltaTime);

            Vector3 movementDirection=(inputDir +environmentDir).normalized;

            if(environmentDir != Vector3.zero ) //환경방향이 있는 경우(ex 슬라이딩)
            {
                if(!isInput)
                    moveSpeed = newEnvironmentForce;
                
                if(stateMachine.ReusableData.MovementInput != Vector2.zero) 
                {
                    isInput=true;
                    if(Vector3.Dot(inputDir, environmentDir)>1)//환경 방향이 같은 경우
                    {
                        moveSpeed = GetMovementSpeed()/2+ newEnvironmentForce;//*Time.deltaTime ;
                        addExternalSpeed=GetMovementSpeed()/2;
                    }
                    else if(Vector3.Dot(inputDir, environmentDir)<-1)
                    {
                        moveSpeed = -GetMovementSpeed() +newEnvironmentForce;//*Time.deltaTime;//환경 방향이 다른 경우
                        addExternalSpeed = -GetMovementSpeed();
                        if(moveSpeed<0) moveSpeed=0f;
                    }  
                }
                else 
                {
                    moveSpeed= Mathf.Lerp(moveSpeed, newEnvironmentForce, Time.deltaTime);
                    //if(speed<0) speed=0f;
                }
            }
            // float targetRotationYAngle = Rotate(movementDirection);

            // Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            SetPlayerRotation(movementDirection);
            stateMachine.Player.characterController.Move(Time.deltaTime* movementDirection* moveSpeed - currentPlayerHorizontalVelocity);

        }
        private Vector3 GetSlopeDirection()
        {
            
            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out slopeHit, stateMachine.Player.characterController.height/2* movementData.SlopeData.slopeForceRayLength,1<<3))
            {
                if(slopeHit.normal ==Vector3.up)
                {
                    slideSpeed = 0f;// Mathf.Lerp(slideSpeed , 0f, Time.deltaTime);
                    return environmentDir;//이전 값 리턴

                }
                Vector3 slopeDir= Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);

                environmentDir = -slopeDir * slideSpeed;//* -slideSpeed;
                environmentDir.y=environmentDir.y -movementData.SlopeData.SlopeForce*100*Time.deltaTime;
                if(environmentDir.y>0) environmentDir.y=0f;
                return environmentDir;
            }
            return Vector3.zero;
        }        
         protected override void AddInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Slide.performed += OnSlideHold;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Slide.performed += OnSlideHold;
            }
        }

        protected override void RemoveInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Slide.performed -= OnSlideHold;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Slide.performed -= OnSlideHold;
            }


        }

        protected override void OnSlideHold(InputAction.CallbackContext obj)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

    
        //나중에 수정할 것
        
        private void SetPlayerRotation(Vector3 dir)
        {
            currentTime+=Time.deltaTime;

            Vector3 lookDir =stateMachine.Player.transform.forward;

            lookDir = Vector3.Lerp(lookDir, dir, 1/currentTime);
            stateMachine.Player.transform.forward= lookDir;    
            stateMachine.Player.transform.eulerAngles= new Vector3(0,stateMachine.Player.transform.eulerAngles.y, stateMachine.Player.transform.eulerAngles.z);
        }
        private void ResetPlayerRotation()
        {
            stateMachine.Player.transform.eulerAngles= new Vector3(0,stateMachine.Player.transform.eulerAngles.y, stateMachine.Player.transform.eulerAngles.z);
        }
    }
}
