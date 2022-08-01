using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerGroundedState : PlayerMovementState
    {

        protected bool shouldSprint;
        protected bool canSlide=true;//임시, 슬라이딩 가능한 경사면이 있으면 슬라이딩으로 할 있도록 trigger에서 체크 
        public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            SlopeForce(movementData.SlopeData.SlopeForce);
        }

        public override void Exit()
        {
            base.Exit();
        }
        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.SprintToggle.started +=OnSprintToggle;
            stateMachine.Player.Input.PlayerActions.Slide.performed += OnSlide;
        }

        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.SprintToggle.started -=OnSprintToggle;
            stateMachine.Player.Input.PlayerActions.Slide.performed -= OnSlide;

        }

        protected virtual void OnMove()
        {
            if(shouldSprint)
                stateMachine.ChangeState(stateMachine.SprintingState);   
            else
                stateMachine.ChangeState(stateMachine.RunningState);   
        }

        public override void OnTriggerExit(Collider other) 
        {
            base.OnTriggerExit(other);
            isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers() || OnSlope();
            if(( ((1 << other.gameObject.layer) & stateMachine.Player.GroundLayers) != 0))
            {
                if(isGrounded)
                    return;
                else
                    OnFall();
            }
        }
        protected void SlopeForce(float force)
        {
            if(stateMachine.ReusableData.MovementInput != Vector2.zero && OnSlope())
            {
                stateMachine.Player.characterController.Move(Vector3.down *GetMovementSpeed()* force *Time.deltaTime);
            }
        }

        protected bool OnSlope()
        {
            //isJumping return false;
            RaycastHit hit;
            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out hit, stateMachine.Player.characterController.height/2.0f* movementData.SlopeData.slopeForceRayLength, 1<<8))
                if(hit.normal != Vector3.up)
                    return true;
            return false;
        }
        #endregion

        #region Input Methods


        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        
        public void OnSprintToggle(InputAction.CallbackContext context)
        {
            shouldSprint = !shouldSprint;
        }
        protected void OnSlide(InputAction.CallbackContext obj)
        {
            shouldSlide = true;
            if(shouldSlide)
                stateMachine.ChangeState(stateMachine.SlidingState);
            
        }
        #endregion
    }
}
