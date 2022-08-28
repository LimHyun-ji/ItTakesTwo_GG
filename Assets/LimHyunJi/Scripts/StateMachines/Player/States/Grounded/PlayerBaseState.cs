using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace ItTakesTwo
{
    public class PlayerBaseState : IState
    {
        protected PlayerMovementStateMachine stateMachine;        

        protected PlayerGroundedData movementData;
        protected CameraController camera;


        public PlayerBaseState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine=playerMovementStateMachine;
            movementData=stateMachine.Player.Data.GroundedData;
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();

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
        public virtual void PhysicsUpdate()
        {
        }
        public virtual void Update()
        {
            
        }
        
        public void LateUpdate(){}//여기서는 안씀
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
            if(collider.gameObject.tag == "Player") return;
            if((1<<collider.gameObject.layer) == LayerMask.GetMask("WallArea"))
            {
                stateMachine.ReusableData.isWallArea = true;
            }

            TriggerSavePoint(collider);
            CheckIsDie(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            //if(((1 << collider.gameObject.layer) & LayerMask.NameToLayer("Magnet")) == 0) return;
            //벽 사이에 있는지 트리거 체크
            if((1<<collider.gameObject.layer) == LayerMask.GetMask("WallArea"))
            {
                stateMachine.ReusableData.isWallArea = false; 
            }

        }

       

        #region Main Methods
        private void ReadMovementInput()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
                stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.Player1Actions.Movement.ReadValue<Vector2>();
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
                stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.Player2Actions.Movement.ReadValue<Vector2>();

        }
        #endregion

        #region ReusableMethods
        protected virtual void AddInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Jump.performed += OnJump;
                stateMachine.Player.Input.Player1Actions.TestForSave.performed += GoToSavePoint;
                stateMachine.Player.Input.Player1Actions.MagnetOn.performed += MagnetOn;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Jump.performed += OnJump;
                stateMachine.Player.Input.Player2Actions.MagnetOn.performed += MagnetOn;

                //stateMachine.Player.Input.Player2Actions.TestForSave.performed += GoToSavePoint;
            }
        }
        protected virtual void RemoveInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.Player1)
            {
                stateMachine.Player.Input.Player1Actions.Jump.performed -= OnJump;
                stateMachine.Player.Input.Player1Actions.TestForSave.performed -= GoToSavePoint;
                stateMachine.Player.Input.Player1Actions.MagnetOn.performed -= MagnetOn;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.Player2)
            {
                stateMachine.Player.Input.Player2Actions.Jump.performed -= OnJump;
                stateMachine.Player.Input.Player2Actions.MagnetOn.performed -= MagnetOn;

                //stateMachine.Player.Input.Player2Actions.TestForSave.performed -= GoToSavePoint;
                
            }
        }

        private void MagnetOn(InputAction.CallbackContext obj)
        {
            stateMachine.Player.isMagnet= !stateMachine.Player.isMagnet;

            if(stateMachine.Player.isMagnet)
            {
                //마그넷 돌려서 앞으로
                ActiveMagnet();
            }
            else
            {
                //마그넷 돌려서 뒤로
                InactiveMagnet();
            }
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
            stateMachine.ReusableData.isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers();            
            //Debug.Log("velocity : "+stateMachine.Player.velocity.y);

            stateMachine.Player.velocity.y += -Time.deltaTime*gravity;
            stateMachine.Player.characterController.Move(stateMachine.Player.velocity* Time.deltaTime);
            if (stateMachine.ReusableData.isGrounded && stateMachine.Player.velocity.y <-1f)// && stateMachine.ReusableData.MovementInput == Vector2.zero )
                stateMachine.Player.velocity.y = -1f; 
        }

        protected void ResetVelocity()
        {
            stateMachine.Player.velocity =Vector3.zero;
        }

        
        protected bool CheckGroundLayers()
        {
            //바닥 1차 체크
            bool grounded =Physics.CheckSphere(stateMachine.Player.groundPivot.transform.position, movementData.groundCheckRadius, stateMachine.Player.GroundLayers, QueryTriggerInteraction.Ignore);
            return grounded;
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if(stateMachine.ReusableData.isWallArea)
            {
                stateMachine.ChangeState(stateMachine.WallJumpingState);
            }
            else
            {
                if(movementData.JumpData.airJumpCount == 0)
                stateMachine.ChangeState(stateMachine.JumpingState);
            }
            
        }
        protected void OnFall()
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        protected void OnActivateDialog()
        {

        }


        protected void TriggerSavePoint(Collider other)
        {
            if( (1<<other.gameObject.layer) == LayerMask.GetMask("SavePoint"))
            {
                if(other.gameObject.tag == stateMachine.Player.playerName.ToString())
                {
                    stateMachine.Player.savePoint = stateMachine.Player.transform.position;
                    other.gameObject.SetActive(false);
                }
            }
        }

        protected void GoToSavePoint(InputAction.CallbackContext context)
        {
            Debug.Log("GoBack Clear");
            // stateMachine.Player.characterController.enabled=false;
            // stateMachine.Player.transform.position=stateMachine.Player.savePoint;
            // stateMachine.Player.characterController.enabled=true;
        }

        protected void GoToSavePoint()
        {
            Debug.Log("GoBack Clear");
            stateMachine.Player.characterController.enabled=false;
            stateMachine.Player.transform.position=stateMachine.Player.savePoint;
            stateMachine.Player.transform.eulerAngles=new Vector3(0, stateMachine.Player.transform.eulerAngles.y, 0);
            stateMachine.Player.characterController.enabled=true;
        }


        protected void CheckIsDie(Collider collider)
        {
            if(1<<collider.gameObject.layer == LayerMask.GetMask("DeadZone"))
            {
                stateMachine.ChangeState(stateMachine.DyingSate);
            }
        }
        #endregion

        protected void AudioPlay(AudioClip clip, bool isLoop, float volume)
        {
            stateMachine.Player.audioSource.Stop();
            stateMachine.Player.audioSource.clip=clip;
            stateMachine.Player.audioSource.loop=isLoop;
            stateMachine.Player.audioSource.volume=volume;
            stateMachine.Player.audioSource.Play();
        }

        public void ActiveMagnet()
        {
            stateMachine.Player.ActiveMagnet(1000, true);
        }
        public void InactiveMagnet()
        {
            stateMachine.Player.ActiveMagnet(-1000, false);
        } 
    }
}
