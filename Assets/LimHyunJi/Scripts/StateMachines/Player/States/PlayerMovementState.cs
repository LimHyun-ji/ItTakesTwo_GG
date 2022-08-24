using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{

    public class PlayerMovementState : PlayerBaseState
    {
        
        public bool isInput;
        protected bool canInteract;

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
            Move();
        }
        public override void HandleInput()
        {
            base.HandleInput();
        }
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);
            UseGravity(9.8f);
            //상호작용 가능한 물체랑 tirgger하면
            if((  (1<<collider.gameObject.layer)  & LayerMask.GetMask("Interactable")) != 0)
            {
                canInteract=true;
                stateMachine.Player.interactableObject=collider.gameObject;
            }
        }

        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            UseGravity(9.8f);
            if((  (1<<collider.gameObject.layer) & LayerMask.GetMask("Interactable")) != 0)
            {
                canInteract=false;
                stateMachine.Player.interactableObject=null;

            }
        }

        #endregion

        #region Main Methods

        protected virtual void Move()
        {
            if(stateMachine.Player.isMovable)
            {
                Vector3 movementDirection;
                
                if ((stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.SpeedModifier == 0f))
                {
                    return; //not moving
                }

                movementDirection=GetMovementInputDirection();
                 
                float speed = GetMovementSpeed();
                
                float targetRotationYAngle = Rotate(movementDirection);

                Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
                Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
                //Debug.Log(speed);
                stateMachine.Player.characterController.Move(Time.deltaTime* targetRotationDirection* speed - currentPlayerHorizontalVelocity);
            }
        }
       
        protected float Rotate(Vector3 inputDir)
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
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Dash.started += OnDashStarted;
                stateMachine.Player.Input.Player1Actions.Interact.performed += OnInteract;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Dash.started += OnDashStarted;
                stateMachine.Player.Input.Player2Actions.Interact.performed += OnInteract;
            }
        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Dash.started -= OnDashStarted;
                stateMachine.Player.Input.Player1Actions.Interact.performed -= OnInteract;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Dash.started -= OnDashStarted;
                stateMachine.Player.Input.Player2Actions.Interact.performed -= OnInteract;
            }
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
        
        protected void OnInteract(InputAction.CallbackContext obj)
        {
                Debug.Log("Arir Interact Hook"+ stateMachine.Player.interactableObject);
            if(!stateMachine.Player.interactableObject) return;
            if(canInteract && stateMachine.Player.interactableObject.tag == "Hook")
            {
                stateMachine.ChangeState(stateMachine.SwingState);
                //interactableObject.GetComponent<SphereCollider>().enabled=false;//multil에서는 이렇게 해도 됨 
            }
            if(canInteract && stateMachine.Player.interactableObject.tag == "RollerCoaster")
            {
                Debug.Log("Arir Interact Roller"+ stateMachine.Player.interactableObject);
                
                stateMachine.ChangeState(stateMachine.RidingState);
                //interactableObject.GetComponent<SphereCollider>().enabled=false;
            }
        }
        
        

        #endregion
    }
}

