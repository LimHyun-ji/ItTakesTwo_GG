using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ItTakesTwo
{
    [System.Serializable]
    public struct PullPushInfo
    {
    }
    
    public class PullPushBase : MonoBehaviour
    {
        #region collider && direction
        public GameObject col;
        private Vector3 dir;
        public Vector3 rayDir;
        #endregion

        #region result
        public bool push;
        public bool pull;
        #endregion

        #region magnet
        public bool magnetNearby;
        #endregion

        #region button
        public ButtonBase button;
        public bool buttonNearby;
        #endregion

        #region player
        public Player player;
        public bool forceDown;
        #endregion

        #region gameObject
        private GameObject entrance;
        #endregion

        protected void Awake()
        {
            player = GetComponentInParent<Player>();
            button = player.GetComponent<ButtonBase>();
            entrance = GameObject.Find("Entrance");
            // print("entrance: " + entrance);

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            MagnetDetect(other);
            ButtonDetect(other);
        }

        protected void OnTriggerStay(Collider other)
        {
            PoleDetect(other);
            
            ButtonOn();
            SawMove();
            JumpPadMove();
        }

        private void MagnetDetect(Collider other)
        {
            if (other.gameObject.name != "Npole" && other.gameObject.name != "Spole")
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                {
                    magnetNearby = true;
                    print("magnetNearBy: " + magnetNearby);
                }
            }
        }

        private void PoleDetect(Collider other)
        {
            rayDir = other.transform.position - transform.position;
            Ray ray = new Ray(transform.position, rayDir);
            RaycastHit hitInfo;

            if (magnetNearby)
            {
                // Sphere detect
                if (button.holding)
                {
                    if (Physics.Raycast(ray, out hitInfo, 100, 1 << LayerMask.NameToLayer("Magnet")))
                    {
                        Debug.DrawRay (ray.origin, ray.direction * 2f, Color.blue, 100f, false);
                        // 2 N/S 극인지 체크
                        // 3 pull / push 여부 결정
                        if (hitInfo.transform.gameObject.tag == gameObject.tag)
                        {
                            print("S tag same");
                            if (col == null)
                            {
                                col = hitInfo.transform.gameObject;
                            }
                            print("S push!");
                            push = true;
                            pull = false;
                        }
                        else
                        {
                            if (col == null)
                            {
                                col = hitInfo.transform.gameObject;
                            }
                            print("S pull!");
                            pull = true;
                            push = false;
                        }
                    }
                }
                else
                {
                    col = null;
                    push = false;
                    pull = false;
                    // EnableTrailR(false);
                }
            }
        }
        
        private void ButtonDetect(Collider other)
        {
            // 버튼주변 - tag: button
            // 버튼자체 - tag: button / layer = magnet 
            if (other.gameObject.tag.Contains("Button"))
            {
                if (col == null)
                {
                    col = other.transform.gameObject;
                    buttonNearby = true;
                    print("buttonNearby: " + buttonNearby);
                }
            }
        }
        
        
        private void ButtonOn()
        {
            if (!buttonNearby)
            {
                return;
            }
            
            forceDown = player.isForceDown;

            if (forceDown)
            {
                // Button 내려간다
                if (col != null && col.gameObject.layer == LayerMask.NameToLayer("ButtonOn"))
                {
                    col.transform.position += col.transform.up * 1.0f * Time.deltaTime;
                    entrance.transform.position += Vector3.up * 5.0f * Time.deltaTime;
                }
            }
        }
        
        private void SawMove()
        {
            if (push && col != null)
            {
                if (col.gameObject.name.Contains("PoleSaw"))
                {
                    if(col.gameObject.transform.parent.name == "Saw")
                    {
                        dir = col.gameObject.transform.forward + Vector3.forward;
                        col.transform.root.position += dir * 1.2f * Time.deltaTime;
                    }
                }
            }
        }
        
        private void JumpPadMove()
        {
            if (push && col != null)
            {
                if (col.gameObject.name.Contains("JumpPad") && !player.isJumppedPad)
                {
                    print("Jump Success");
                    //여기에 점프 패드 변수 가져오기
                    player.isJumppedPad=true;
                    player.movementStateMachine.ChangeState(player.movementStateMachine.JumpingState);
                    
                    player.velocity.y = 30;
                    // EnableTrailR(true);
                }
            }
        }
    }
}
