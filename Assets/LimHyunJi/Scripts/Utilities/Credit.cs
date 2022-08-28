using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class Credit : MonoBehaviour
    {
        RectTransform rect;
        Vector2 anchoredPos= new Vector2(0,0);
        public float scrollSpeed=50f;

        // Start is called before the first frame update
        void Start()
        {
            rect=GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if(anchoredPos.y<3900)
            {
                anchoredPos.y +=  scrollSpeed*Time.deltaTime;
                rect.anchoredPosition= anchoredPos;

            }

        }
    }
}
