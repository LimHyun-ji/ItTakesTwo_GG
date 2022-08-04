using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

// 머리가 안돌아가... ㅠㅠㅠ
namespace ItTakesTwo
{
    public class SawMove : MonoBehaviour
    {
        // private float speed = 3f;
        // private float forwardSpeed = 0.3f;
        private Vector3 startRight;
        private Vector3 startLeft;
        private Vector3 startForward;

        private PullPushMagnetS pmS;
        private PullPushMagnetN pmN;
        
        // Start is called before the first frame update
        void Start()
        {
            startRight = transform.right;
            startLeft = transform.right * -1;
            startForward = transform.forward;
            pmS = GameObject.Find("Spole").GetComponent<PullPushMagnetS>();
            pmN = GameObject.Find("Npole").GetComponent<PullPushMagnetN>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        private float currentTime;
        private float rotateTime = 1f;
        private float turn;
        public void UpdateSMove()
        {
            currentTime += Time.deltaTime;
            if (currentTime > rotateTime)
            {
                pmS.push = false;
                currentTime = 0;
            }
            else
            {
                print("UpdateSMove");
                turn = Mathf.Lerp(0, 15f, currentTime);
                transform.rotation = Quaternion.Euler(0, -turn, 0);
                transform.position += startForward * Time.deltaTime;
                transform.position += startRight * Time.deltaTime;
            }
        }
                
        private float curTime;
        private float rotTime = 1f;
        private float turnN;
        public void UpdateNMove()
        {
            curTime += Time.deltaTime;
            if (curTime > rotTime)
            {
                pmN.push = false;
                curTime = 0;
            }
            else
            {
                // print("UpdateNMove");
                turn = Mathf.Lerp(0, 15f, curTime);
                transform.rotation = Quaternion.Euler(0, turn, 0);
                transform.position += startForward * Time.deltaTime;
                transform.position += startLeft * Time.deltaTime;
            }
        }
    }
}
