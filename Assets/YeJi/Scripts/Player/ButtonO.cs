using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class ButtonO : MonoBehaviour
    {
        public bool bO;
        
        // Start is called before the first frame update
        void Start()
        {
            bO = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                bO = true;
                print("button o pushed");
            }
            else
            {
                bO = false;
            }
        }
    }
}
