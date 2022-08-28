using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
namespace ItTakesTwo
{
    [RequireComponent(typeof(PlayerInput))]//자동으로 해당 스크립트를 컴포넌트로 붙여줌
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data {get; private set;}
        
        public enum PlayerType {Player1, Player2};
        public PlayerType playerName;

        [HideInInspector]
        public CharacterController characterController;
        [HideInInspector]
        public bool isMovable;
        public Rigidbody rigidBody{get; private set;}
        public Vector3 velocity;
        public LayerMask GroundLayers;
        public GameObject groundPivot;
        public Transform mainCameraTransform;//{get; private set;}
        [HideInInspector]
        public Transform cameraLookPoint;
        [HideInInspector]
        public GameObject interactableObject;
        public PlayerInput Input{get; protected set;}
        public PlayerMovementStateMachine movementStateMachine;
        [HideInInspector]
        public Animator animator;
        public Animator dummyAnimator;
        [HideInInspector]
        public Vector3 savePoint;
        [HideInInspector]
        public bool isJumppedPad;
        public GameObject model;
        public GameObject magnet;
        //임시
        public GameObject wall_0;
        public GameObject wall_1;
        [HideInInspector]
        public bool isForceDown;
        [HideInInspector]
        public  Vector3 cameraDir;
        [HideInInspector]
        public AudioSource audioSource;
        [HideInInspector]
        public GameObject rope;
        private Vector3 initMagnetPos;
        [HideInInspector]
        public bool isMagnet;




        protected virtual void Awake() 
        {
            characterController=GetComponent<CharacterController>();
            rigidBody =GetComponent<Rigidbody>();
            Input=GetComponent<PlayerInput>();
           
            movementStateMachine=new PlayerMovementStateMachine(this);
            animator=GetComponentInChildren<Animator>();
            audioSource =GetComponent<AudioSource>();

            rope= Instantiate(Resources.Load<GameObject>("Prefabs_HJ/Rope"));
        }
        protected virtual void Start() 
        {
            savePoint= transform.position;

            Input=GetComponent<PlayerInput>();
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
            cameraLookPoint=transform.Find("CameraLookPoint");
            initMagnetPos=magnet.transform.localPosition;

            isMovable=true;
        }
        protected virtual void Update() 
        {
            movementStateMachine.HandleInput();
            movementStateMachine.Update();
        }
        protected virtual void FixedUpdate() 
        {
            movementStateMachine.PhysicsUpdate();
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
        }

        public void ActiveMagnet(float angle, bool isActive)
        {
            StopAllCoroutines();
            StartCoroutine(IActiveMagnet(angle, isActive));
        }

        IEnumerator IActiveMagnet(float angle, bool isActive)
        {
            if(isActive)
            {
                while(magnet.transform.eulerAngles.x<180)
                {
                    magnet.transform.RotateAround(transform.position, transform.right, angle *Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                magnet.transform.localEulerAngles=new Vector3(180,0,0);
            }
            else
            {
                while(magnet.transform.eulerAngles.x<=180)// || magnet.transform.eulerAngles.x)
                {
                    magnet.transform.RotateAround(transform.position, transform.right, angle *Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                magnet.transform.localEulerAngles=new Vector3(0,0,0);
                magnet.transform.localPosition= initMagnetPos;

            }
        }


    }
}