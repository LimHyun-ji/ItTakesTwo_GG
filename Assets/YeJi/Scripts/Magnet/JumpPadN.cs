using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 같은 성질이면
// player를 위로 이동시킨다
// 가속도 필요

namespace ItTakesTwo
{
    public class JumpPadN : MonoBehaviour
    {
        private GameObject cody;
        private PullPushMagnetN pmN;
        private float speed = 8.0f;
        
        // Start is called before the first frame update
        void Start()
        {
            cody = GameObject.Find("Cody");
            pmN = GameObject.Find("Npole").GetComponent<PullPushMagnetN>();
            print("pmN" + pmN);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (pmN.push)
            {
                print("pmN.push");
                // 위치 값 보내기
                // Player jumpPad state 추가해야할 듯
            }
        }
    }
}
