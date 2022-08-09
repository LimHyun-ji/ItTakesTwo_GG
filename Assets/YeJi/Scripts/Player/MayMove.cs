using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ItTakesTwo
{
    public class MayMove : MonoBehaviour
    {
        public CharacterController cc;
        private float speed = 5.0f;
        private float gravity = -9.81f;
        public float jumpPower = 5f;
        public float yVelocity;
        public Vector3 dir;
        private PullPushMagnet pm;
        
        // Start is called before the first frame update
        void Awake()
        {
            cc = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            dir = new Vector3(h, 0, v);

            yVelocity += gravity * Time.deltaTime;
            if (cc.isGrounded)
            {
                yVelocity = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
            pm= GetComponentInChildren<PullPushMagnet>();
            
            //예지 추가
            if (pm.canJump)
            {
                yVelocity = 7;
            }


            dir.y = yVelocity;
            
            cc.Move(dir * speed * Time.deltaTime);
        }
    }
}
