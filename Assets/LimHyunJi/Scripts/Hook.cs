using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Hook : MonoBehaviour
    {
        private GameObject target;
        Player player;

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindWithTag("Player");
            player=target.GetComponent<Player>();
            player.isMovable=false;
        }   

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
