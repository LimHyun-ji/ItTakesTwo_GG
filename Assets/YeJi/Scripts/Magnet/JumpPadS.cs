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
    public class JumpPadS : MonoBehaviour
    {
        private GameObject may;
        private PullPushMagnetS pmS;
        private float speed = 8.0f;
        
        // Start is called before the first frame update
        void Start()
        {
            may = GameObject.Find("May");
            pmS = GameObject.Find("Spole").GetComponent<PullPushMagnetS>();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (pmS.push)
            {
                // 위치 값 보내기
                // Player jumpPad state 추가해야할 듯
            }
        }
    }
}
