using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.PlayerLoop;

namespace ItTakesTwo
{
    public class ButtonE : ButtonBase
    {
        void Start()
        {
            input = KeyCode.E;
        }
    }
}
