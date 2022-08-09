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
        protected bool shouldSlide;
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
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.Movement.canceled += OnMovementCanceled;
                stateMachine.Player.Input.Player1Actions.SprintToggle.started +=OnSprintToggle;
                stateMachine.Player.Input.Player1Actions.Slide.performed += OnSlideHold;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.Movement.canceled += OnMovementCanceled;
                stateMachine.Player.Input.Player2Actions.SprintToggle.performed +=OnSprintToggle;
                stateMachine.Player.Input.Player2Actions.Slide.performed += OnSlideHold;
            }
        }

        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.Movement.canceled -= OnMovementCanceled;
                stateMachine.Player.Input.Player1Actions.SprintToggle.started -=OnSprintToggle;
                stateMachine.Player.Input.Player1Actions.Slide.performed -= OnSlideHold;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.Movement.canceled -= OnMovementCanceled;
                stateMachine.Player.Input.Player2Actions.SprintToggle.started -=OnSprintToggle;
                stateMachine.Player.Input.Player2Actions.Slide.performed -= OnSlideHold;
            }


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
            if( OnSlope())
            {
                stateMachine.Player.characterController.Move(Vector3.down *GetMovementSpeed()* force *Time.deltaTime);
            }
        }

        protected bool OnSlope()//원래 layer 1<< 8로 ground에 ray를 쏴야 정상인데 그냥 몸통에다가 쏴서 움직이지 slope Force를 추가해주는 격
        {
            //isJumping return false;
            RaycastHit hit;
            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out hit, stateMachine.Player.characterController.height/2.0f* movementData.SlopeData.slopeForceRayLength))
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
        protected virtual void OnSlideHold(InputAction.CallbackContext obj)
        {
            //shouldSlide = true;
            stateMachine.ChangeState(stateMachine.SlidingState);
        }
        #endregion
    }
}
