using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CameraBaseState : IState
    {
        CameraMovementStateMachine stateMachine;
        public CameraBaseState(CameraMovementStateMachine cameraMovementStateMachine)
        {
            stateMachine=cameraMovementStateMachine;
        }
        public void Enter()
        {
        }
        public void HandleInput()
        {
            //cameraInput = stateMachine.Camera.Input.Player1Actions.Look.ReadValue<Vector2>();
            //playerMovementInput = stateMachine.Camera.Input.Player1Actions.Movement.ReadValue<Vector2>();
        }
        public void LateUpdate()
        {

        }
        public void PhysicsUpdate()
        {
        }

        public void Update()
        {
        }
        public void OnTriggerEnter(Collider collider)
        {
        }

        public void OnTriggerExit(Collider collider)
        {
        }
        public void Exit()
        {
        }

    }
}
