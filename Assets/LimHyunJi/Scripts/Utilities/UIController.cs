using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace ItTakesTwo
{
    public class UIController : MonoBehaviour
    {

        public Image startImage;
        Image fadeImage;
        public bool isStartClicked=false;
        
        private static UIController _instance;
        public static UIController Instance()
        {
            return _instance;
        }
        private void Awake() 
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            startImage = GameObject.Find("StartImage").GetComponent<Image>();
            fadeImage =GameObject.Find("FadeImage").GetComponent<Image>();
        }

        void OnEnable()
        {
            // 씬 매니저의 sceneLoaded에 체인을 건다.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FadeOut();
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0) && !isStartClicked)
            {
                //처음 시작시 버튼 누르면 fadein되면서 다음씬으로 넘어가기
                isStartClicked=true;
                FadeIn();
            }
        }

        private void FadeOut()
        {   
            StopAllCoroutines();
            StartCoroutine(IFadeOut());
        }

        private void FadeIn()
        {   
            StopAllCoroutines();
            StartCoroutine(IFadeIn());
        }
        IEnumerator IFadeOut()//(float startAlpha, float endAlpha, float deltaTime)
        {
            Color alPhaColor= new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

            while(alPhaColor.a>0)
            {
                alPhaColor.a-=Time.deltaTime;
                fadeImage.color=alPhaColor;
                yield return null;
            }
            
        }

        IEnumerator IFadeIn()//(float startAlpha, float endAlpha, float deltaTime)
        {
            Color alPhaColor= new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);

            while(alPhaColor.a<1)
            {
                alPhaColor.a+=Time.deltaTime;
                fadeImage.color=alPhaColor;
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Load");
            SceneManager.LoadScene(1);
            startImage.gameObject.SetActive(false);
        }
    }
}
