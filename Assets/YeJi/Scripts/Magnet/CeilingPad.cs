using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CeilingPad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            print("x: " + transform.position.x);
            print("y: " + transform.position.y);
            print("z: " + transform.position.z);
        }
    }
}
