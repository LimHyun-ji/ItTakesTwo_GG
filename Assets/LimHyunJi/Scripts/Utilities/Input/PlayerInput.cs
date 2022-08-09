using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions {get; protected set;}
        public PlayerInputActions.Player1Actions Player1Actions{get; private set;}
        public PlayerInputActions.Player2Actions Player2Actions{get; private set;}

        //public struct PlayerActions{};
        //public PlayerActions playerActions;
        protected virtual void Awake() 
        {
            InputActions = new PlayerInputActions();

            Player1Actions = InputActions.Player1;
            Player2Actions=InputActions.Player2;
        }

        private void OnEnable() 
        {
            InputActions.Enable();
        }
        private void OnDisable() 
        {
            InputActions.Disable();  
        }
    }
    
}
