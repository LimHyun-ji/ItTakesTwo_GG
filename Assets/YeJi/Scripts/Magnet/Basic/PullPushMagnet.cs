using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
namespace ItTakesTwo
{
    // 캐릭터컨트롤러로 가져오고 완료
    // 중력 초기화 완료
    // dir 전역변수로 설정 => 여기서 변경
    // Move() -> lateUpdate()
    public class PullPushMagnet : MonoBehaviour
    {
        #region push / pull @RETURN bool
        public bool push;
        public bool pull;
        #endregion

        #region Player / button
        // private GameObject may;
        // private GameObject cody;
        private Player player;
        private ButtonE buttonE;
        private ButtonO buttonO;
        #endregion

        #region collider && direction
        private Vector3 dir;
        private Collider col;
        private float rot;
        private bool leverUp;
        #endregion

        public bool shootRay;
        
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponentInParent<Player>();
            
            buttonE = GameObject.Find("Player1").GetComponent<ButtonE>();
            buttonO = GameObject.Find("Player2").GetComponent<ButtonO>();
        }

        void Update()
        {
            // shootRay = true;
            // 확인용
            // print("yVelocity : " + yVelocity);
            // print("push " + push);
            // print("pull " + pull);
            // print("col " + col);
            // print("canJump " + canJump);
            // print(cc.isGrounded);
        }

        private void OnTriggerStay(Collider other)
        {
            Detect(other);
            
            // Let collider move
            SawMove(other);
            JumpPadMove(other);
            CeilingPadMove(other);
            LeverMove(other);
            // RingBell(other);
        }

        private void RingBell(Collider other)
        {
            if (other.gameObject.name.Contains("BellInside"))
            {
                if (pull && col != null)
                {
                    other.gameObject.transform.forward = player.transform.forward;
                    other.gameObject.transform.right = player.transform.right;
                    float x = 0;
                    float z = 0;
                    // if (other.gameObject.transform.parent.position.x - 20 <= x &&
                    //     x <= other.gameObject.transform.parent.position.x + 20 &&
                    //     other.gameObject.transform.parent.position.z - 20 <= z &&
                    //     z <= other.gameObject.transform.parent.position.z)
                    // {
                        x = player.transform.position.x;
                        z = player.transform.position.z;
                        other.gameObject.transform.position =
                            new Vector3(player.transform.position.x, other.gameObject.transform.position.y, player.transform.position.z);
                    // }
                }
            }
        }
        
        private void Detect(Collider other)
        {
            // Sphere detect
            if (buttonE.eHolding || buttonO.oHolding)
            {
                // 1 자석인지 체크
                if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                {
                    // 2 N/S 극인지 체크
                    // 3 pull / push 여부 결정
                    if (other.gameObject.tag == gameObject.tag)
                    {
                        print("tag same");
                        if (col == null)
                        {
                            col = other;
                        }
                        print("push!");
                        push = true;
                        pull = false;
                    }
                    else
                    {
                        if (col == null)
                        {
                            col = other;
                        }
                        print("pull!");
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
            }
        }

        private void SawMove(Collider other)
        {
            if (other.gameObject.name.Contains("PoleSaw"))
            {
                if(other.gameObject.transform.parent.name == "Saw")
                {
                    if (push && col != null)
                    {
                        dir = col.gameObject.transform.forward + Vector3.forward;
                        col.transform.root.position += dir * 2.0f * Time.deltaTime;
                    }
                }
            }
        }
        
        private void JumpPadMove(Collider other)
        {
            if (other.gameObject.name.Contains("JumpPad"))
            {
                if (push && col != null)
                {
                    print("jumpPad-push");

                    player.velocity.y += 5;
                }
            }
        }
        
        private void CeilingPadMove(Collider other)
        {
            if (other.gameObject.name.Contains("CeilingPad"))
            {
                if (pull && col != null)
                {
                    print("Ceiling-push");
                    player.velocity.y = other.gameObject.transform.position.y;
                }
            }
        }
        
        private void LeverMove(Collider other)
        {
            if (other.gameObject.name.Contains("SpoonLever"))
            {
                if(other.gameObject.transform.parent.name == "Lever")
                {
                    if (pull && col != null)
                    {
                        rot = 20;
                        rot = Mathf.Lerp(0, rot, Time.deltaTime);
                        if (col.transform.root.eulerAngles.x <= 20)
                        {
                            leverUp = true;
                            col.transform.root.eulerAngles += new Vector3(rot, 0, 0);
                        }
                    }
                    if (!buttonO.oHolding)
                    {
                        other.transform.root.eulerAngles = Vector3.Lerp(other.transform.root.eulerAngles, new Vector3(0, other.transform.root.eulerAngles.y, other.transform.root.eulerAngles.z), Time.deltaTime*15);
                        if (player.gameObject.name == "Player1")
                        {
                            player.velocity.y += 8;
                        }
                        // StartCoroutine(IETurnOff());
                    }
                }
            }
        }

        IEnumerator IETurnOff()
        {
            yield return new WaitForSeconds(1);

            push = false;
            pull = false;
            col = null;
        }
    }
}
