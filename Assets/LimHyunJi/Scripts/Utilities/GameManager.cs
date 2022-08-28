using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. SceneManagement;

namespace ItTakesTwo
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance()
        {
            return _instance;
        }

        private void Awake() 
        {
            if(_instance==null)
            {
                _instance=this;
                DontDestroyOnLoad(this.gameObject);
            }
        }


        void OnEnable()
        {
            // 씬 매니저의 sceneLoaded에 체인을 건다.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
            if(scene.buildIndex == 1)//MainScene
            {
                //임시로 설정 처음 씬 시작시 시네머신 활성화되도록
                CameraManager.Instance().ActivateCineMachine(true);
            }
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        void Start()
        {
        
        }

        void Update()
        {
        
        }
        

    }
}
