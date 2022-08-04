using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class MayMove : MonoBehaviour
    {
        private CharacterController cc;
        private float speed = 5.0f;
        private float gravity = -9.81f;
        private float yVelocity = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            cc = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            yVelocity += gravity * Time.deltaTime;

            Vector3 dir = new Vector3(h, 0, v);
            dir.y = yVelocity;

            cc.Move(dir * speed * Time.deltaTime);
        }
    }
}
