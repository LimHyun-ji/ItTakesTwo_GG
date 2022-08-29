using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ItTakesTwo
{
    public class PullPushBase : MonoBehaviour
    {
        #region collider && direction
        public GameObject col;
        protected Collider detectCol;
        private Vector3 dir;
        public Vector3 rayDir;
        #endregion

        #region result
        public bool push;
        public bool pull;
        public bool bJumpInput;
        public bool bSideInput;
        private bool isJump;
        private bool isSide;
        #endregion

        #region magnet
        public bool magnetNearby;
        private bool isUsingMagnet;
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

        #region time
        private float currentTime;
        private float createTime = 0.3f;
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
            JumpDetect(other);
            SideDetect(other);
        }

        protected void Update()
        {
            PoleDetect();
            
            ButtonOn();
            SawMove();
            if (bJumpInput && button.once)
            {
                isJump = true;
            }
            if(isJump)
                JumpPadMove();

            if (bSideInput && button.once)
            {
                isSide = true;
                print("isSide: " + isSide);
            }
            if (isSide)
            {
                SidePadMove();
            }
        }


        private void MagnetDetect(Collider other)
        {
            if (other.gameObject.name != "Npole" && other.gameObject.name != "Spole")
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                {
                    magnetNearby = true;
                    print("magnetNearBy: " + magnetNearby);
                    detectCol = other;
                }
            }
        }

        private void PoleDetect()
        {
            if (detectCol == null)
            {
                return;
            }
            
            rayDir = detectCol.transform.position - transform.position;
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
                    isUsingMagnet = false;
                    // EnableTrailR(false);
                }
            }
        }

        private void JumpDetect(Collider other)
        {
            rayDir = other.transform.position - transform.position;
            Ray ray = new Ray(transform.position, rayDir);
            RaycastHit hitInfo;

            if (magnetNearby)
            {
                if (Physics.Raycast(ray, out hitInfo, 100, 1 << LayerMask.NameToLayer("Magnet")))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 2f, Color.blue, 100f, false);

                    if (hitInfo.transform.name.Contains("JumpPad") && hitInfo.transform.gameObject.tag == gameObject.tag)
                    {
                        bJumpInput = true;
                    }
                }
            }
        }

        private void SideDetect(Collider other)
        {
            if (magnetNearby)
            {
                if (other.gameObject.tag != gameObject.tag && other.gameObject.layer==LayerMask.NameToLayer("Magnet"))
                {
                    Debug.Log("SidePad tag" +other.gameObject.tag );
                    // sidePad를 찾는다
                    rayDir = other.transform.position - transform.position;
                    Ray ray = new Ray(transform.position, rayDir);
                    RaycastHit hitInfo;
                    
                    if (Physics.Raycast(ray, out hitInfo, 100, 1 << LayerMask.NameToLayer("Magnet")))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.blue, 100f, false);

                        if (hitInfo.transform.name.Contains("SidePad"))
                        {
                            bSideInput = true;
                            // print("bSideInput" + bSideInput);
                        }
                    }
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
                    entrance.transform.position += Vector3.up * 20.0f * Time.deltaTime;
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
                        isUsingMagnet = true;

                        dir = col.gameObject.transform.forward + Vector3.forward;
                        col.transform.root.position += dir * 1.2f * Time.deltaTime;
                    }
                }
            }
        }

        private void JumpPadMove()
        {
            if(isJump)
            {
                player.isJumppedPad=true;
                player.movementStateMachine.ChangeState(player.movementStateMachine.JumpingState);
            }
            currentTime += Time.deltaTime;
            if (currentTime > createTime)
            {
                currentTime = 0;
                
                isUsingMagnet = true;
                
                print("Jump Success");
                //여기에 점프 패드 변수 가져오기
                
                player.velocity.y = 30;
                

                player.isJumppedPad=false;
                isJump = false;
                bJumpInput = false;
                magnetNearby = false;
                // EnableTrailR(true);
            }
        }
        
        private void SidePadMove()
        {
            currentTime += Time.deltaTime;
            if (currentTime > createTime)
            {
                // player.isJumppedPad=true;
                // player.movementStateMachine.ChangeState(player.movementStateMachine.JumpingState);
                // player.velocity.y = 0;
                
                currentTime = 0;
                bSideInput = false;
                magnetNearby = false;
                isSide = false;
                player.characterController.enabled=true;
                // player2.GetComponent<Player>().velocity = sideN.transform.position;
            }
            else
            {
                // 현지코드로 변경
                Debug.Log("Move");
                player.characterController.enabled=false;
                //player.GetComponent<Player>().characterController.Move(rayDir.normalized * 20 * Time.deltaTime);
                player.GetComponent<Player>().velocity.y=0f;
                player.transform.position+= rayDir.normalized * 100 * Time.deltaTime;
            }


        }
    }
}
