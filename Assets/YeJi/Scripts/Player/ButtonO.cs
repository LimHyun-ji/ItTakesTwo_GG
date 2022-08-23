using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItTakesTwo
{
    public class ButtonO : MonoBehaviour
    {
        public bool oHolding=false;
        public bool oOnce;
        Player player;
        
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
        void Update()
        {
            // if (Input.GetKey(KeyCode.Keypad9) || Input.GetButton("Fire1"))
            // {
            //     oHolding = true;
            // }
            // if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetButton("Fire1"))
            // {
            //     print("buttonO");
            //     oOnce = true;
            // }
            // if (Input.GetKeyUp(KeyCode.Keypad9)|| Input.GetButton("Fire1"))
            // {
            //     oHolding = false;
            // }
        }
        public void MagnetOn(InputAction.CallbackContext context)
        {
            Debug.Log("MagnetOff");
            //oHolding = true;
            oOnce = true;
            oHolding = !oHolding;
        }
    }
}
