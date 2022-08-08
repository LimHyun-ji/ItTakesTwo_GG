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
            if(collider.gameObject.tag =="Player") return;
            if(((1 << collider.gameObject.layer) & stateMachine.Player.GroundLayers) != 0)
            {
                OnLand();
            }
            //상호작용 가능한 물체랑 tirgger하면
            if(((1<<collider.gameObject.layer) & LayerMask.NameToLayer("Interactable")) == 0)
            {
                canInteract=true;
                interactableObject=collider.gameObject;
            }

        }
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);
            if(((1<<collider.gameObject.layer) & LayerMask.NameToLayer("Interactable")) == 0)
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

            Player1Input p1Input = stateMachine.Player.Input as Player1Input;
            
            // if(p1Input != null)
            // {
            //     []
            // }
            // else
            // {

            // }

            stateMachine.Player.Input.PlayerActions.DownForce.performed += OnDownForce;
            stateMachine.Player.Input.PlayerActions.Interact.performed += OnInteract;
        }
        protected override void RemoveInputActionsCallBacks()
        {
            base.RemoveInputActionsCallBacks();
            stateMachine.Player.Input.PlayerActions.DownForce.performed -= OnDownForce;
            stateMachine.Player.Input.PlayerActions.Interact.performed -= OnInteract;
        }

        protected void OnInteract(InputAction.CallbackContext obj)
        {
            if(!interactableObject) return;
            if(canInteract && interactableObject.tag == "Hook")
            {
                stateMachine.ChangeState(stateMachine.SwingState);
                interactableObject.GetComponent<SphereCollider>().enabled=false;
            }
            if(canInteract && interactableObject.tag == "RollerCoaster")
            {
                Debug.Log("Arir Interact Roller"+ interactableObject);
                stateMachine.ChangeState(stateMachine.RidingState);
                interactableObject.GetComponent<SphereCollider>().enabled=false;
            }
        }

        public void OnDownForce(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.ForceDownState);
        }
        #endregion
    }
}
