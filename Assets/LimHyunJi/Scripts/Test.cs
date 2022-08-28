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
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform .position += (move * Time.deltaTime * speed);
        }

    }
}
