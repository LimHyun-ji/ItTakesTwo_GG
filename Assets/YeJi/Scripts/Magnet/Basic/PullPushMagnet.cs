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
        private GameObject may;
        private GameObject cody;
        private ButtonE buttonE;
        private ButtonO buttonO;
        private CharacterController cc;
        private float yVelocity;
        public bool canJump;
        #endregion

        #region collider && direction
        private Vector3 dir;
        private Collider col;
        private float rot;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            may = GameObject.Find("Player1");
            cody = GameObject.Find("Player2");
            buttonE = may.GetComponent<ButtonE>();
            buttonO = cody.GetComponent<ButtonO>();
        }

        void Update()
        {
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
            // SawMove(other);
            // LeverMove(other);
            JumpPadMove(other);
            // CeilingPadMove(other);
        }

        
        private void CeilingPadMove(Collider other)
        {
            // 실링패드
            // ray를 위로 쏴서
            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hitInfo;
            
            // layer가 magnet으로 나와 같고
            // if (Physics.Raycast(ray, out hitInfo, 1.5f, 1 << LayerMask.NameToLayer("Magnet")))
            // {
                // Debug.DrawRay (ray.origin, ray.direction * 2f, Color.red, 100f, false);
                print("hitInfo");
                // 게임오브젝트 이름이 CeilingPad이고
                // tag가 다르면
                // pull한다
            // }
            
            
            if (other.gameObject.name.Contains("CeilingPad"))
            {
                if (push && col != null)
                {
                    print("push");
                    canJump = true;

                    StartCoroutine("IETurnOffPad");
                }
            }
        }

        private void Detect(Collider other)
        {
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

            /*
            if (buttonE.eOnce || buttonO.oOnce)
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
            }*/
        }

        private void SawMove(Collider other)
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

        private void LeverMove(Collider other)
        {
            if(other.gameObject.transform.parent.name == "Lever")
            {
                if (pull && col != null)
                {
                    rot = 20;
                    rot = Mathf.Lerp(0, rot, Time.deltaTime);
                    if (col.transform.root.eulerAngles.x <= 20)
                    {
                        col.transform.root.eulerAngles += new Vector3(rot, 0, 0);
                    }
                }
                if (!buttonO.oHolding)
                {
                    other.transform.root.eulerAngles = Vector3.Lerp(other.transform.root.eulerAngles, new Vector3(0, other.transform.root.eulerAngles.y, other.transform.root.eulerAngles.z), Time.deltaTime*15);
                }
            }
        }
        
        private void JumpPadMove(Collider other)
        {
            if (other.gameObject.name.Contains("JumpPad"))
            {
                if (push && col != null)
                {
                    print("push");
                    canJump = true;

                    StartCoroutine("IETurnOffPad");
                }
            }
        }

        IEnumerator IETurnOffPad()
        {
            yield return new WaitForSeconds(2);

            canJump = false;
            push = false;
            col = null;
        }
    }
}
