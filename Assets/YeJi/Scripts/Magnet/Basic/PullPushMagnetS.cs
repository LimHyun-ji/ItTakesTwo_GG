using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
namespace ItTakesTwo
{
    
    public class PullPushMagnetS : MonoBehaviour
    {
        public bool push;
        public bool pull;

        private ButtonE buttonE;
        public 
        
        // Start is called before the first frame update
        void Start()
        {
            push = false;
            buttonE = GameObject.Find("May").GetComponent<ButtonE>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            // 1 Ray를 쏘고 있는 상태에서
            if (Physics.Raycast(ray, out hitInfo, 2f))
            {
                BasicRay(ray, hitInfo);
            }
            
            Ray downRay = new Ray(transform.position, transform.up * -1);
            RaycastHit hitDownInfo;
            if (Physics.Raycast(downRay, out hitDownInfo, 1.5f, 1 << LayerMask.NameToLayer("JumpPadS")))
            {
                DownRay(downRay, hitDownInfo);
            }
        }
        
        private void BasicRay(Ray ray, RaycastHit hitInfo)
        {
            Debug.DrawRay (ray.origin, ray.direction * 2f, Color.red, 100f, false);
            // 2 E 버튼을 눌렀을 때
            if (buttonE.bE)
            {
                if (hitInfo.transform.gameObject.layer == gameObject.layer)
                {
                    // 4 bool push 을 true로 반환한
                    push = true;
                    pull = false;
                }
                else
                {
                    pull = true;
                    push = false;
                }
            }
        }

        private void DownRay(Ray ray, RaycastHit hitInfo)
        {
            Debug.DrawRay (ray.origin, ray.direction * 2f, Color.yellow, 100f, false);

            if (buttonE.bE)
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("JumpPadS"))
                {
                    print("pullPushMagnet : push == true");
                    push = true;
                    pull = false;
                }
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("JumpPadN"))
                {
                    // print("pull == true");
                    pull = true;
                    push = false;
                }
            }
        }
    }
}
