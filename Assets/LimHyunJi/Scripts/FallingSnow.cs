using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class FallingSnow : MonoBehaviour
    {
        public GameObject target;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.position=target.transform.position+ new Vector3(0, 10, 0);
        }
    }
}
