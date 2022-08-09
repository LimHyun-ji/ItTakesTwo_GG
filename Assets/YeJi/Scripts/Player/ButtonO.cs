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
            if (Input.GetKey(KeyCode.O))
            {
                oHolding = true;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                oOnce = true;
            }
            if (Input.GetKeyUp(KeyCode.O))
            {
                oHolding = false;
            }
        }
    }
}
