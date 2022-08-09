using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace ItTakesTwo
{
    public class SawMove : MonoBehaviour
    {
        #region position
        private Vector3 treeFstPos;
        private GameObject treeTop;
        #endregion

        #region Time
        private float createTime = 1;
        private float currentTime;
        #endregion
        
        void Start()
        {
            treeTop = GameObject.Find("TreeTop");
            treeFstPos = treeTop.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            CutTree();
        }

        private void CutTree()
        {
            if (treeTop.transform.position.z < transform.position.z)
            {
                // print("tree down!!");
                //tree의 z값을 높인다
                treeTop.transform.position += Vector3.forward * 30 * Time.deltaTime;
                currentTime += Time.deltaTime;
                if (currentTime < createTime)
                {
                    Vector3.Lerp(treeFstPos, treeFstPos + transform.forward, currentTime * 0.5f);
                }
            }
        }
    }
}
