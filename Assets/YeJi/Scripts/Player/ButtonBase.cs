using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class ButtonBase : MonoBehaviour
    {
        public bool holding=false;
        public bool once = false;

        public KeyCode input;

        private void Update()
        {
            if (Input.GetKey(input))
            {
                holding = true;
            }

            if (Input.GetKeyDown(input))
            {
                once = true;
            }

            if (Input.GetKeyUp(input))
            {
                holding = false;
                once = false;
            }
        }
    }
}
