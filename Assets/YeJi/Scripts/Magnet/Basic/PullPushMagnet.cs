using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
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
        private GameObject col;
        private float rot;
        private GameObject lever;
        #endregion

        private bool magnetNearby;
        private Vector3 rayDir;
        private TrailRenderer[] trailRends;
        // [SerializeField] private LineRenderer lineRend;
        
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponentInParent<Player>();
            // trailRends = GameObject.Find("LaserEffects").GetComponentsInChildren<TrailRenderer>();
            
            buttonE = GameObject.Find("Player1").GetComponent<ButtonE>();
            buttonO = GameObject.Find("Player2").GetComponent<ButtonO>();

            // lineRend = GetComponent<LineRenderer>();
        }

        void Update()
        {
            // shootRay = true;
            // 확인용
            // print("magnetNearby: " + magnetNearby);
            // print("yVelocity : " + yVelocity);
            // print("push " + push);
            // print("pull " + pull);
            // print("col " + col);
            // print("canJump " + canJump);
            // print(cc.isGrounded);
            // print("trailRends: " + trailRends.Length);
        }

        private void OnTriggerStay(Collider other)
        {
            MagnetDetect(other);
            PoleDetect(other);
            
            // Let collider move
            SawMove();
            JumpPadMove();
            CeilingPadMove();
            LeverMove();
            // RingBell(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "Npole" && other.gameObject.name != "Spole")
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                {
                    // e/9 버튼 범위
                    magnetNearby = false;
                    player.velocity.z = 0;
                }
            }
        }
        
        
        private void MagnetDetect(Collider other)
        {
            if (other.gameObject.name != "Npole" && other.gameObject.name != "Spole")
            {
                // 1 근처에 magnet이 있는지 감지
                if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                {
                    // e/9 버튼 범위
                    magnetNearby = true;
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
                if (buttonE.eHolding || buttonO.oHolding)
                {
                    if (Physics.Raycast(ray, out hitInfo, 100, 1 << LayerMask.NameToLayer("Magnet")))
                    {
                        Debug.DrawRay (ray.origin, ray.direction * 2f, Color.red, 100f, false);
                        // 2 N/S 극인지 체크
                        // 3 pull / push 여부 결정
                        if (hitInfo.transform.gameObject.tag == gameObject.tag)
                        {
                            print("tag same");
                            if (col == null)
                            {
                                col = hitInfo.transform.gameObject;
                            }
                            print("push!");
                            push = true;
                            pull = false;
                        }
                        else
                        {
                            if (col == null)
                            {
                                col = hitInfo.transform.gameObject;
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
                    // EnableTrailR(false);
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
                        col.transform.root.position += dir * 2.0f * Time.deltaTime;
                    }
                }
            }
        }

        private void JumpPadMove()
        {
            if (push && col != null)
            {
                if (col.gameObject.name.Contains("JumpPad"))
                {
                    // EnableTrailR(true);
                    player.velocity.y += 10;
                }
            }
        }

        private void CeilingPadMove()
        {
            if (pull && col != null)
            {
                if (col.gameObject.name.Contains("CeilingPad"))
                {
                    print("Ceiling-push");
                    // EnableTrailR(true);
                    player.velocity.y = col.gameObject.transform.position.y;
                }
            }
        }

        private void SidePadMove(Collider other)
        {
            if (pull && col != null)
            {
                if (other.gameObject.name.Contains("SidePad"))
                {
                    print("Side-push");
                    player.velocity.z = other.gameObject.transform.position.z;
                }
            }
        }
        private void LeverMove()
        {
            if (pull && col != null )
            {
                if (col.gameObject.name.Contains("SpoonLever"))
                {
                    if (col.gameObject.transform.parent.name == "Lever")
                    {
                        lever = col.transform.parent.gameObject;
                        rot = 20;
                        rot = Mathf.Lerp(0, rot, Time.deltaTime);
                        if (col.transform.root.eulerAngles.x <= 20)
                        {
                            col.transform.root.eulerAngles += new Vector3(rot, 0, 0);
                        }
                    }

                }
            }
            if (!buttonO.oHolding && lever != null)
            {
                print("11111111111111");
                print("lever.transform.eulerAngles: " + lever.transform.eulerAngles);
                
                lever.transform.eulerAngles = Vector3.Lerp(lever.transform.eulerAngles, new Vector3(0, lever.transform.eulerAngles.y, lever.transform.eulerAngles.z), Time.deltaTime*15);
                        
                // 지렛대가 올라가면
                // 지렛대에 있는 플레이어가
                // 포물선 방향으로 날아간다
                if (player.gameObject.name == "Player1")
                {
                    player.velocity.y += 15;
                    player.velocity.z += 40;
                }
            }
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

        private void EnableTrailR(bool trueOrFalse)
        {
            foreach (TrailRenderer trailRend in trailRends)
            {
                trailRend.enabled = trueOrFalse;
            }
        }
    }
}
