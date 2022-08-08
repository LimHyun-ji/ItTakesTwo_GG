using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Player1Input : PlayerInput
    {
        public PlayerInputActions.Player1Actions PlayerActions{get; set;}

        protected override void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player1;
        }
    }
}
