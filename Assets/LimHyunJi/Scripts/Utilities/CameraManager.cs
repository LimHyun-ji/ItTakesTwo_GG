using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace ItTakesTwo
{
    public class CameraManager : MonoBehaviour
    {
        private Rect cineViewPort=new Rect(0, 0, 1, 1);
        private Rect gameViewPort=new Rect(0,0,0.5f,1f);
        Camera mainCamera;
        public Camera secondCamera;//inspector에서 할당
        //private List<CinemachineVirtualCamera> virtualCamers = new List<CinemachineVirtualCamera>();
        GameObject virtualCamera;
        CinemachineBrain cineBrain;
        // Start is called before the first frame update
        bool isActive;

        private static CameraManager _instance;
        public static CameraManager Instance()
        {
            return _instance;
        }
        private void Awake() 
        {
            _instance=this;

            Init();
            
        }
        void Init()
        {
            mainCamera=Camera.main;
            //virtual camera들을 자식으로 담는 오브젝트
            virtualCamera=GameObject.FindGameObjectWithTag("VirtualCamera");
            cineBrain = mainCamera.gameObject.GetComponent<CinemachineBrain>();

            virtualCamera.SetActive(false);
        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void ActivateCineMachine(bool isActive)
        {
            if(UIController.Instance())
                    UIController.Instance().SetDialogHeight(isActive);
            //CineMachine Manager 는 게임 오브젝트 껏다가 이때 켜기
            //화면 비율 1개 카메라로
            if(isActive)
                mainCamera.rect=cineViewPort;
            else
            {
                SetCamWidth(gameViewPort);
                //virtual camera 자식들 중 가장 마지막 카메라의 위치로 할것(현재는 임시)
                mainCamera.gameObject.GetComponent<CameraController>().CameraLookTransform.eulerAngles=virtualCamera.transform.eulerAngles;
                mainCamera.gameObject.GetComponent<CameraController>().SetMouseRotationInit(virtualCamera.transform);
                secondCamera.gameObject.GetComponent<CameraController>().CameraLookTransform.eulerAngles=virtualCamera.transform.eulerAngles;
                secondCamera.gameObject.GetComponent<CameraController>().SetMouseRotationInit(virtualCamera.transform);

                
            }

            cineBrain.enabled=isActive;
            virtualCamera.SetActive(isActive);
        }
        //시네머신 끝날 때
        private void SetCamWidth(Rect viewPort)
        {
            StartCoroutine(ISetCamWidth(gameViewPort));
        }

        private IEnumerator ISetCamWidth(Rect viewPort)
        {
            Rect tempViewPort= mainCamera.rect;
            //main width cine일때는 1, game할 땐 0.5
            while(mainCamera.rect.width>viewPort.width)
            {
                tempViewPort.width -= Time.deltaTime/(1.5f);
                mainCamera.rect=tempViewPort;
                yield return new WaitForEndOfFrame();
            }
            mainCamera.rect=viewPort;
        }
    }
}
