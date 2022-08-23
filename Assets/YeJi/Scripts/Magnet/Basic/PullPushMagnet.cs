using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        private GameObject player1;
        private GameObject player2;
        private ButtonE buttonE;
        private ButtonO buttonO;
        #endregion

        #region collider && direction
        private Vector3 dir;
        private GameObject col;
        private float rot;
        private GameObject lever;
        private bool jump;
        #endregion

        #region magent
        private bool magnetNearby;
        private Vector3 rayDir;
        private TrailRenderer[] trailRends;
        private GameObject sideN;
        private GameObject sideS;
        private bool sidePushN;
        private bool sidePushS;
        Vector3 dirN;
        Vector3 dirS;
        #endregion
        
        #region button
        private bool buttonNearby;
        public bool forceDown;
        private Vector3 fstPos;
        private GameObject entrance;
        #endregion

        // [SerializeField] private LineRenderer lineRend;
        
        // Start is called before the first frame update
        void Start()
        {
            player = GetComponentInParent<Player>();
            player1 = GameObject.Find("Player1");
            player2 = GameObject.Find("Player2");
            // trailRends = GameObject.Find("LaserEffects").GetComponentsInChildren<TrailRenderer>();
            
            buttonE = GameObject.Find("Player1").GetComponent<ButtonE>();
            buttonO = GameObject.Find("Player2").GetComponent<ButtonO>();
            
            sideN = GameObject.Find("SidePadN");
            sideS = GameObject.Find("SidePadS");
            
            entrance = GameObject.Find("Entrance");
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
            // print("player: " + player);
            // print("canJump " + canJump);
            // print(cc.isGrounded);
            // print("trailRends: " + trailRends.Length);

            SidePadDetect();
            SidePadMove();
        }
        private void OnTriggerEnter(Collider other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
            MagnetDetect(other);
            PoleDetect(other);
            ButtonDetect(other);

            // Let collider move
            SawMove();
            JumpPadMove();
            CeilingPadMove();
            LeverMove();
            ButtonOn();
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
                    buttonNearby = false;
                    player.velocity.z = 0;
                    player.isJumppedPad=false;
                    push=false;
                    pull=false;

                }

                if (other.gameObject.layer == LayerMask.NameToLayer("ButtonOn"))
                {
                    player.isForceDown=false;
                    forceDown = false;
                    col = null;
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
            forceDown = player.isForceDown;
            // player 상태 == PlayerForceDownState (추후 현지한테 요청)
            if (forceDown)
            {
                //fstPos = col.transform.position;
                // Button 내려간다
                if ( col != null && col.gameObject.layer == LayerMask.NameToLayer("ButtonOn"))
                {
                    col.transform.position += col.transform.up * 1.0f * Time.deltaTime;
                    entrance.transform.position += Vector3.up * 5.0f * Time.deltaTime;
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
                    sidePushN = false;
                    sidePushS = false;
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
                    //문제 1. 근처에 있을때 계속 뜀
                    player.isJumppedPad=true;
                    player.movementStateMachine.ChangeState(player.movementStateMachine.JumpingState);
                    
                    player.velocity.y = 30;
                    // EnableTrailR(true);
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

        private void SidePadDetect()
        {
            // sidePad를 찾는다
            
            float distanceS = Vector3.Distance(sideS.transform.position , player1.transform.position);
            float distanceN = Vector3.Distance(sideN.transform.position , player2.transform.position);

            // sidePad의 거리가 일정거리이하면
            if (distanceS < 30)
            {
                if (buttonE.eHolding)
                {
                    sidePushS = true;
                    dirS = sideS.transform.position - player1.transform.position;
                }
                else
                {
                    sidePushS = false;
                }
            }
            // sidePad의 거리가 일정거리이하면
            if (distanceN < 30)
            {
                if (buttonO.oHolding)
                {
                    if (sidePushN == false)
                    {
                        dirN = sideN.transform.position - player2.transform.position;
                    }
                    sidePushN = true;
                    
                }
                else
                {
                    sidePushN = false;
                }
            }
        }

        private void SidePadMove()
        {
            if (sidePushN)
            {
                print("player2 sidepad move");
                
                // 현지코드로 변경
                player2.GetComponent<Player>().characterController.Move(dirN.normalized * 20 * Time.deltaTime);
                player2.GetComponent<Player>().velocity.y=0f;
                // player2.GetComponent<Player>().velocity = sideN.transform.position;
            }
            if (sidePushS)
            {
                                
                // 현지코드로 변경
                // player1.GetComponent<Player>().characterController.enabled = false;
                // player1.transform.position += dirS.normalized * 20 * Time.deltaTime;
                // print("player1 sidepad move");
                // player1.transform.position += dirS.normalized * 20 * Time.deltaTime;

                player1.GetComponent<Player>().characterController.Move(dirS.normalized * 20 * Time.deltaTime);
                player1.GetComponent<Player>().velocity.y=0f;

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
                        rot = 500;
                        rot = Mathf.Lerp(0, rot, Time.deltaTime);
                        if (col.transform.root.eulerAngles.x <= 300)
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
                if (player.gameObject.name == "Player2")
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
