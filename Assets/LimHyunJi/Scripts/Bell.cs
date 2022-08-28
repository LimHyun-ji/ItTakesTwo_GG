using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Bell : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag=="Player")
            {
                if(UIController.Instance())
                {
                    Debug.Log("PlayVideo");
                    GameObject.Find("Environment").GetComponent<AudioSource>().enabled=false;
                    UIController.Instance().PlayVideo(UIController.Instance().endingVideo);
                }
            }
        }
    }
}
