using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerBaseState : IState
    {
        protected PlayerMovementStateMachine stateMachine;
        protected static bool isGrounded;

        protected PlayerGroundedData movementData;
        public PlayerBaseState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine=playerMovementStateMachine;
            movementData=stateMachine.Player.Data.GroundedData;

            InitializedData();
        }
        private void InitializedData()
        {
            stateMachine.ReusableData.TimeToReachTargetRotation=movementData.BaseRotationData.targetRotationReachTime;
        }
        public virtual void Enter()
        {
            Debug.Log("State"+ GetType().Name);
            AddInputActionsCallBacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallBacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
        }

        public virtual void OnTriggerExit(Collider collider)
        {
        }

        public virtual void PhysicsUpdate()
        {
        }
        public virtual void Update()
        {
        }

        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }
        #endregion

        #region ReusableMethods
        protected virtual void AddInputActionsCallBacks()
        {
            
        }
        protected virtual void RemoveInputActionsCallBacks()
        {
            
        }
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

        protected void UseGravity(float gravity)
        {
            isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers();
            
            stateMachine.Player.velocity.y += -Time.deltaTime*gravity;
            stateMachine.Player.characterController.Move(stateMachine.Player.velocity* Time.deltaTime);
            // if (isGrounded && stateMachine.Player.velocity.y <0f)// && stateMachine.ReusableData.MovementInput == Vector2.zero )
            //     stateMachine.Player.velocity.y = 0f; 
        }

        protected void ResetVelocity()
        {
            stateMachine.Player.velocity =Vector3.zero;
        }

        
        protected bool CheckGroundLayers()
        {
            bool grounded =Physics.CheckSphere(stateMachine.Player.groundPivot.transform.position, movementData.groundCheckRadius, stateMachine.Player.GroundLayers, QueryTriggerInteraction.Ignore);
            return grounded;
        }
        
        #endregion

    }
}
