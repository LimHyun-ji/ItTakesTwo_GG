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
        Vector3 movementDir;
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedModifier=movementData.SlopeData.speedModifier;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            //Move(Vector3.right);
            SlopeMove();
        }

        private void SlopeMove()
        {
            stateMachine.Player.characterController.Move(GetEnvironmentDir());

        }

        public override void Exit()
        {
            base.Exit();
        }
        protected override void Move(Vector3 environmentDir)
        {
            base.Move(environmentDir);

            //stateMachine.Player.characterController.Move(GetEnvironmentDir() *5f* Time.deltaTime);
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
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        protected Vector3 GetEnvironmentDir()
        {
            RaycastHit hit;

            if(Physics.Raycast(stateMachine.Player.transform.position, Vector3.down, out hit, stateMachine.Player.characterController.height/2 + 1f))
            {
                // Vector3 slopeDir = Vector3.down-hit.normal*Vector3.Dot(Vector3.up, hit.normal);
                // slopeDir.Normalize();
                    if(hit.normal != Vector3.up)
                        return Vector3.Cross(hit.normal, hit.point);
            }
            return Vector3.zero;              
        }
    }
}
