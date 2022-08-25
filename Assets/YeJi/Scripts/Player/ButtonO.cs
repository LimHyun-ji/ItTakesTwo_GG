using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.WindowsMR.Input;

namespace ItTakesTwo
{
    public class ButtonO : ButtonBase
    {
        Player player;
        
        void Start()
        {
            input = KeyCode.Keypad9; 
        }

        // Start is called before the first frame update
        void Awake()
        {
            player = GetComponentInParent<Player>();
        }
        void OnEnable()
        {
            player.Input.Player2Actions.MagnetOn.performed += MagnetOn;
        }
        void OnDisable()
        {
            player.Input.Player2Actions.MagnetOn.performed -= MagnetOn;
        }

        // Update is called once per frame
        public void MagnetOn(InputAction.CallbackContext context)
        {
            Debug.Log("MagnetOff");
            //oHolding = true;
            // oOnce = true;
            once = !once;
            holding = !holding;
        }
    }
}
