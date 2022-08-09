using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class ButtonO : MonoBehaviour
    {
        public bool oHolding;
        public bool oOnce;
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Keypad9))
            {
                oHolding = true;
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                oOnce = true;
            }
            if (Input.GetKeyUp(KeyCode.Keypad9))
            {
                oHolding = false;
            }
        }
    }
}
