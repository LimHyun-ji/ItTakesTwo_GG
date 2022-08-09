using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace ItTakesTwo
{
    public class ButtonE : MonoBehaviour
    {
        public bool eHolding;
        public bool eOnce;
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                eHolding = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                eOnce = true;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                eHolding = false;
            }
        }
    }
}
