using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CameraController : MonoBehaviour
    {
        private GameObject target;
        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if(target)
            {

            }
        }

        
    }
}
