using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
namespace ItTakesTwo
{
    
    public class PullPushMagnetN : MonoBehaviour
    {
        public bool push;
        public bool pull;

        private ButtonO buttonO;
        
        // Start is called before the first frame update
        void Start()
        {
            push = false;
            buttonO = GameObject.Find("Cody").GetComponent<ButtonO>();
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
            if (Physics.Raycast(downRay, out hitDownInfo, 1f, 1 << LayerMask.NameToLayer("JumpPadN")))
            {
                DownRay(downRay, hitDownInfo);
            }
        }

        private void BasicRay(Ray ray, RaycastHit hitInfo)
        {
            Debug.DrawRay (ray.origin, ray.direction * 2f, Color.red, 100f, false);
            // 2 E 버튼을 눌렀을 때
            if (buttonO.bO)
            {
                // print("ButtonE.buttonE");
                // 3 Layer 이름이 서로 동일하면
                // print("hitInfo.name : "+hitInfo.transform.gameObject.name);
                // print("hitInfo.transform.gameObject.layer : " + hitInfo.transform.gameObject.layer);
                    
                // print("gameObject.name : " + gameObject.name);
                // print("gameObject.layer : " + gameObject.layer);
                    
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

            if (buttonO.bO)
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("JumpPadN"))
                {
                    print("push == true");
                    push = true;
                    pull = false;
                }
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("JumpPadS"))
                {
                    print("pull == true");
                    pull = true;
                    push = false;
                }
            }
        }
    }
}
