using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerBaseState : IState
    {
        protected PlayerMovementStateMachine stateMachine;
        protected static bool isGrounded;
        protected bool isWallArea;

        public static GameObject interactableObject;

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
            if(collider.gameObject.tag == "Player") return;
            //if(((1 << collider.gameObject.layer) & LayerMask.NameToLayer("Magnet")) == 0) return;
            isWallArea = WallAreaCheck(collider);
            Debug.Log(" IsWall"+ isWallArea);

        }

        public virtual void OnTriggerExit(Collider collider)
        {
            //if(((1 << collider.gameObject.layer) & LayerMask.NameToLayer("Magnet")) == 0) return;
            isWallArea = !WallAreaCheck(collider);

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
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
                stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.Player1Actions.Movement.ReadValue<Vector2>();
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
                stateMachine.ReusableData.MovementInput=stateMachine.Player.Input.Player2Actions.Movement.ReadValue<Vector2>();

        }
        #endregion

        #region ReusableMethods
        protected virtual void AddInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.Jump.performed += OnJump;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.Jump.performed += OnJump;
            }
        }
        protected virtual void RemoveInputActionsCallBacks()
        {
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.Jump.performed -= OnJump;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.Jump.performed -= OnJump;
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
            isGrounded=stateMachine.Player.characterController.isGrounded || CheckGroundLayers();
            //Debug.Log("velocity : "+stateMachine.Player.velocity.y);

            stateMachine.Player.velocity.y += -Time.deltaTime*gravity;
            stateMachine.Player.characterController.Move(stateMachine.Player.velocity* Time.deltaTime);
            if (isGrounded && stateMachine.Player.velocity.y <-1f)// && stateMachine.ReusableData.MovementInput == Vector2.zero )
                stateMachine.Player.velocity.y = -1f; 
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
        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log("isWallArea " +isWallArea);
            if(isWallArea)
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
        //벽 사이에 들어가는지 트리거 체크
        protected bool WallAreaCheck(Collider collider)
        {
            if(1<<collider.gameObject.layer == LayerMask.GetMask("WallArea"))
            {
                return true;
            }
            else 
                return false;
        }
        
        #endregion

    }
}
