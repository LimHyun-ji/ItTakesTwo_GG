using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace ItTakesTwo
{
    public class GameManager : MonoBehaviour
    {

        Image startImage;
        Image fadeImage;
        public Sequence sequenceFadeIn;

        void Start()
        {
            startImage = GameObject.Find("Canvas").transform.Find("StartImage").GetComponent<Image>();
            fadeImage =GameObject.Find("Canvas").transform.Find("FadeImage").GetComponent<Image>();

            startImage.enabled=false;
            //fadeImage.gameObject.SetActive(false);

            DOTween.Init();

            sequenceFadeIn=DOTween.Sequence()
            .SetAutoKill(false)
            //.AppendCallback(()=>{startImage.color=Color.black;startImage.gameObject.SetActive(true);})
            .Append(startImage.DOFade(0.0f, 1f))
            .Pause();
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
               sequenceFadeIn.Rewind();
               sequenceFadeIn.Play();
            }
        }

        private void FadeIn()
        {
            startImage.DOFade(0f, 1f);
        }
    }
}
