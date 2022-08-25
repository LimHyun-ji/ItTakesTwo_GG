using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class InteractableUI : MonoBehaviour
    {
        GameObject ropeUI;
        GameObject magnetUI;
        public GameObject[] players;
        GameObject myUI;

        private void Awake() 
        {
            ropeUI = Resources.Load<GameObject>("Prefabs_HJ/RopeUI");
            magnetUI=Resources.Load<GameObject>("Prefabs_HJ/MagnetUI");

            players=GameObject.FindGameObjectsWithTag("Player");

            if(gameObject.tag=="Hook" || gameObject.tag=="RollerCoaster")
            {
                myUI=Instantiate(ropeUI);
            }
            else if( 1 << gameObject.layer ==LayerMask.GetMask("Magnet"))
            {
                myUI=Instantiate(magnetUI);
            }
        }

        private void Update() 
        {
            //03순서 중요
            float distance1 =Vector3.Distance(this.gameObject.transform.position, players[0].transform.position);
            float distance2 =Vector3.Distance(this.gameObject.transform.position, players[3].transform.position);
            if(distance1 <10 || distance2<10)
            {
                myUI.SetActive(true);
                myUI.transform.position=transform.position + -myUI.transform.forward/2;
            }
            else
            {
                myUI.SetActive(false);
            }
        }
    }
}
