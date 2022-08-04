using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Test : MonoBehaviour
    {
        float speed=3f;
        public Vector3 _velocity;
        private CharacterController _controller;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }
            
        void Update()
        {
            Debug.Log(_controller.isGrounded);
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _controller.Move(move * Time.deltaTime * speed);

            // _velocity.y += -9.8f * Time.deltaTime;
            //  _controller.Move(_velocity * Time.deltaTime);
        }

    }
}
