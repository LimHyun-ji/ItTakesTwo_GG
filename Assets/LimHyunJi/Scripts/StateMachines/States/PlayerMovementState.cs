using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerMovementState : PlayerBaseState
    {
        protected static bool shouldSlide;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            UseGravity(9.8f);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            UseGravity(9.8f);
            Move(Vector3.zero);
        }
        public override void HandleInput()
        {
            base.HandleInput();
        }
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            UseGravity(9.8f);
        }

        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            UseGravity(9.8f);
        }

        #endregion

        #region Main Methods

        protected virtual void Move(Vector3 environmentDir)
        {
            if(stateMachine.Player.isMovable)
            {
                Vector3 movementDirection;

                
                if ((stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.SpeedModifier == 0f) && environmentDir == Vector3.zero)
                {
                    return; //not moving
                }

                movementDirection = GetMovementInputDirection()+ environmentDir;
                float targetRotationYAngle = Rotate(movementDirection);

                Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
                float movementSpeed = GetMovementSpeed();
                Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

                stateMachine.Player.characterController.Move(Time.deltaTime* targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity);
            }            
        }
       
        private float Rotate(Vector3 inputDir)
        {
            float directionAngle = UpdateTargetRotation(inputDir);

            RotateTowardsTargetRotation();
            return directionAngle;
        }
       
        private float GetDirectionAngle(Vector3 inputDir)
        {
            float directionAngle= Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            if(directionAngle <0)
                directionAngle +=360f;
            
            return directionAngle;
        }
        private float AddCameraRotationAngle(float angle)
        {
            angle += stateMachine.Player.mainCameraTransform.eulerAngles.y;
            if(angle >360)
                angle -= 360f;

            return angle;
        }
        
        private void UpdateTargetRotationData(float targetAngle)
        {
            stateMachine.ReusableData.CurrentTargetRotation.y=targetAngle;
            stateMachine.ReusableData.DampedTargetRotationPassedTime.y=0f;
        }

        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Jump.started += OnJump;
            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJump;
            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        }
        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle=stateMachine.Player.transform.rotation.eulerAngles.y;

            if(currentYAngle ==stateMachine.ReusableData.CurrentTargetRotation.y)
                return;
                
            float smoothYAngle=Mathf.SmoothDampAngle(currentYAngle,stateMachine.ReusableData.CurrentTargetRotation.y, ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,stateMachine.ReusableData.TimeToReachTargetRotation.y-stateMachine.ReusableData.DampedTargetRotationPassedTime.y);
                    stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation =Quaternion.Euler(0f, smoothYAngle, 0f);

            stateMachine.Player.transform.rotation=targetRotation;
        }
        protected float UpdateTargetRotation(Vector3 inputDir, bool shouldConsiderCameraRotation=true)
        {
            float directionAngle =  GetDirectionAngle(inputDir);
            if(shouldConsiderCameraRotation)
            {
                directionAngle  =  AddCameraRotationAngle(directionAngle);
            }

            if(directionAngle !=stateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }
            return directionAngle;
        }
        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }


        
        protected void OnDashStarted(InputAction.CallbackContext context)
        {
            if(movementData.DashData.airDashCount == 0)
                stateMachine.ChangeState(stateMachine.DashingState);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if(movementData.JumpData.airJumpCount == 0)
                stateMachine.ChangeState(stateMachine.JumpingState);
        }

        protected void OnFall()
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        #endregion
    }
}

