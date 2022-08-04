using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace ItTakesTwo
{
    public class ButtonE : MonoBehaviour
    {
        public bool bE;
        public float curY;
        
        // Start is called before the first frame update
        void Start()
        {
            bE = false;
            curY = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bE = true;
                print("button e pushed");
            }
            else
            {
                bE = false;
            }
        }
    }
}
