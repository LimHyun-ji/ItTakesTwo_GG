using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Player2Input : PlayerInput
    {
        public PlayerInputActions.Player2Actions PlayerActions{get;set;}

        protected override void Awake()
        {
            InputActions = new PlayerInputActions();
            PlayerActions = InputActions.Player2;
        }
    }
}
