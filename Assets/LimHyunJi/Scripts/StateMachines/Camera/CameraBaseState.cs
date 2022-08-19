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
            ReadMouseInput();
        }
        public void LateUpdate()
        {
            Look();
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

        private void ReadMouseInput()
        {
            Debug.Log("Read");
            stateMachine.ReusableData.mouseInput.x += Input.GetAxis("Mouse X")*stateMachine.ReusableData.mouseSpeed;
            stateMachine.ReusableData.mouseInput.y += Input.GetAxis("Mouse Y")*stateMachine.ReusableData.mouseSpeed;
        }

        protected void Look()
        {
            SetMouseY();
            SetCameraTargetAngle();
        }
        private void SetMouseY()
        {
            stateMachine.ReusableData.mouseInput.y=Mathf.Clamp(stateMachine.ReusableData.mouseInput.y, -60f, 60f);
        }
        private void SetCameraTargetAngle()
        {
            stateMachine.Camera.CameraLookTransform.eulerAngles =new Vector3(stateMachine.ReusableData.mouseInput.x,stateMachine.ReusableData.mouseInput.y, 0);
        }
        

    }
}
