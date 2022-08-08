using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions {get; protected set;}
        public PlayerInputActions.Player1Actions PlayerActions{get; private set;}
        
        //public struct PlayerActions{};
        //public PlayerActions playerActions;
        protected virtual void Awake() 
        {
            InputActions = new PlayerInputActions();

            PlayerActions = InputActions.Player1;
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
