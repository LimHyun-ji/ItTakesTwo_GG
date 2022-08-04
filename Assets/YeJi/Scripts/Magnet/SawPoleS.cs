using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class SawPoleS : MonoBehaviour
    {
        private SawMove sm;
        private PullPushMagnetS pmS;
        
        // Start is called before the first frame update
        void Start()
        {
            // 2 Saw 게임오브젝트를 가져와서
            sm = GetComponentInParent<SawMove>();
            
            pmS = GameObject.Find("Spole").GetComponent<PullPushMagnetS>();
            // print("dir" + dir);     //-1
        }

        // Update is called once per frame
        void Update()
        {
            // 1 만약 PullPushMagnet.push = true 라면
            if (pmS.push)
            {
                // 3 오른쪽으로 이동시킨다
                sm.UpdateSMove();
            }
        }
    }
}
