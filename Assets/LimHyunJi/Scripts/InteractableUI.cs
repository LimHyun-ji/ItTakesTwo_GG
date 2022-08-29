using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class InteractableUI : MonoBehaviour
    {
        GameObject ropeUI;
        GameObject magnetUI_N;
        GameObject magnetUI_S;
        public GameObject[] players;
        private GameObject myUI;
        public float minDis=3;
        public float maxDis=15;

        private void Awake() 
        {
            ropeUI = Resources.Load<GameObject>("Prefabs_HJ/RopeUI");
            magnetUI_N=Resources.Load<GameObject>("Prefabs_HJ/MagnetUI_N");
            magnetUI_S=Resources.Load<GameObject>("Prefabs_HJ/MagnetUI_S");

            players=GameObject.FindGameObjectsWithTag("Player");

            if(gameObject.tag=="Hook" || gameObject.tag=="RollerCoaster")
            {
                myUI=Instantiate(ropeUI);
            }
            else if( 1 << gameObject.layer ==LayerMask.GetMask("Magnet"))
            {
                if(gameObject.tag =="Spole")
                    myUI=Instantiate(magnetUI_S);
                if(gameObject.tag =="Npole")
                    myUI=Instantiate(magnetUI_N);
            }
            //if(myUI)
                myUI.SetActive(false);
        }

        private void Update() 
        {
            //03순서 중요
            float distance1 =Vector3.Distance(this.gameObject.transform.position, players[0].transform.position);
            float distance2 =Vector3.Distance(this.gameObject.transform.position, players[3].transform.position);
            if((distance1 <maxDis && distance1>minDis)|| (distance2<maxDis && distance2>maxDis))
            {
                myUI.SetActive(true);
                myUI.transform.position=transform.position ;//+ -myUI.transform.forward/2;
                myUI.transform.eulerAngles= transform.eulerAngles;
            }
            else
            {
                myUI.SetActive(false);
            }
        }
    }
}
