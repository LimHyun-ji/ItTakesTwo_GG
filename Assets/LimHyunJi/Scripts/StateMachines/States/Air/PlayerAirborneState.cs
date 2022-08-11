using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class PlayerAirborneState : PlayerMovementState
    {
        protected bool canFly;
        protected bool canInteract;
        
        public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void OnTriggerEnter(Collider collider) 
        {
            base.OnTriggerEnter(collider);
            if(((1 << collider.gameObject.layer) & stateMachine.Player.GroundLayers) != 0)
            {
                OnLand();
            }
            //상호작용 가능한 물체랑 tirgger하면
            if((  (1<<collider.gameObject.layer)  & LayerMask.GetMask("Interactable")) != 0)
            {
                canInteract=true;
                interactableObject=collider.gameObject;
            }

        }
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            if((  (1<<collider.gameObject.layer) & LayerMask.GetMask("Interactable")) != 0)
            {
                canInteract=false;
                interactableObject=null;

            }
        }

        private void OnLand()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        #region  Reusable Methods
        protected override void AddInputActionsCallBacks()
        {
            base.AddInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.DownForce.performed += OnDownForce;
                stateMachine.Player.Input.Player1Actions.Interact.performed += OnInteract;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.DownForce.performed += OnDownForce;
                stateMachine.Player.Input.Player2Actions.Interact.performed += OnInteract;
            }


        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            if(stateMachine.Player.playerName == Player.PlayerType.player1)
            {
                stateMachine.Player.Input.Player1Actions.DownForce.performed -= OnDownForce;
                stateMachine.Player.Input.Player1Actions.Interact.performed -= OnInteract;
            }
            else if(stateMachine.Player.playerName == Player.PlayerType.player2)
            {
                stateMachine.Player.Input.Player2Actions.DownForce.performed -= OnDownForce;
                stateMachine.Player.Input.Player2Actions.Interact.performed -= OnInteract;
            }
        }

        protected void OnInteract(InputAction.CallbackContext obj)
        {
                Debug.Log("Arir Interact Hook"+ interactableObject);
            if(!interactableObject) return;
            if(canInteract && interactableObject.tag == "Hook")
            {
                stateMachine.ChangeState(stateMachine.SwingState);
                //interactableObject.GetComponent<SphereCollider>().enabled=false;//multil에서는 이렇게 해도 됨 
            }
            if(canInteract && interactableObject.tag == "RollerCoaster")
            {
                Debug.Log("Arir Interact Roller"+ interactableObject);
                stateMachine.ChangeState(stateMachine.RidingState);
                //interactableObject.GetComponent<SphereCollider>().enabled=false;
            }
        }

        public void OnDownForce(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.ForceDownState);
        }
        #endregion
    }
}
