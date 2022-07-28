using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;

        protected PlayerGroundedData movementData;
        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine=playerMovementStateMachine;
            movementData=stateMachine.Player.Data.GroundedData;

            InitializedData();
        }
        private void InitializedData()
        {
            stateMachine.ReusableData.TimeToReachTargetRotation=movementData.BaseRotationData.targetRotationReachTime;
        }
        
        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State"+ GetType().Name);
            AddInputActionsCallBacks();
        }

    
        public virtual void Exit()
        {
            RemoveinputActionsCallBacks();
        }

        public virtual void HandleInut()
        {
            ReadMovementInput();
        }
        public virtual void Update()
        {
        }
        public virtual void PhysicsUpdate()
        {
            Debug.Log(GetPlayerVerticalVelocity());
            Move();
            UseGravity();
        }
        public virtual void OnTriggerEnter(Collider other) 
        {

        }
        public virtual void OnTriggerExit(Collider other) 
        {
        }
        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        #endregion

        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }
        private void Move()
        {
            Vector3 movementDirection;
            
            if (((stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.SpeedModifier == 0f)))
            {
                return; //not moving
            }

            movementDirection = GetMovementInputDirection();
            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.characterController.Move(Time.deltaTime* targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity);
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
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f,stateMachine.ReusableData.MovementInput.y);
        }
        protected float GetMovementSpeed()
        {
            return movementData.baseSpeed * stateMachine.ReusableData.SpeedModifier;
        }
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalvelocity = stateMachine.Player.characterController.velocity;
            playerHorizontalvelocity.y=0f;

            return playerHorizontalvelocity;
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
             return new Vector3(0f, stateMachine.Player.characterController.velocity.y, 0f);
        }
        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle=stateMachine.Player.transform.rotation.eulerAngles.y;

            if(currentYAngle ==stateMachine.ReusableData.CurrentTargetRotation.y)
                return;
                
            float smoothYAngle=Mathf.SmoothDampAngle(currentYAngle,stateMachine.ReusableData.CurrentTargetRotation.y, ref         stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,stateMachine.ReusableData.TimeToReachTargetRotation.y-stateMachine.ReusableData.DampedTargetRotationPassedTime.y);
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

        protected void UseGravity()
        {
            stateMachine.Player.velocity.y += Time.deltaTime*-9.8f;
            stateMachine.Player.characterController.Move(stateMachine.Player.velocity* Time.deltaTime);

            bool isGrounded= CheckGroundLayers();
            if (isGrounded &&  stateMachine.Player.velocity.y < 0)
                stateMachine.Player.velocity.y = 0f;
        }

        protected void ResetVelocity()
        {
            stateMachine.Player.velocity =Vector3.zero;
        }

        protected virtual void AddInputActionsCallBacks()
        {

        }

    
        protected virtual void RemoveinputActionsCallBacks()
        {
        }

     
        protected virtual bool CheckGroundLayers()
        {
            bool grounded =Physics.CheckSphere(stateMachine.Player.transform.position, movementData.groundCheckRadius, stateMachine.Player.GroundLayers, QueryTriggerInteraction.Ignore);
            return grounded;
        }
        #endregion
    }
}

