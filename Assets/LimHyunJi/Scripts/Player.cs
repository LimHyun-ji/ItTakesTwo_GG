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
        
        public Rigidbody rigidBody{get; private set;}
        public LayerMask GroundLayers;
        public Transform mainCameraTransform{get; private set;}
        [HideInInspector]
        public Transform cameraLookPoint;
        public PlayerInput Input{get; private set;}
        private PlayerMovementStateMachine movementStateMachine;


        private void Awake() 
        {
            rigidBody =GetComponent<Rigidbody>();
            Input=GetComponent<PlayerInput>();
            movementStateMachine=new PlayerMovementStateMachine(this);
            mainCameraTransform=Camera.main.transform;
        }
        protected virtual void Start() 
        {
            Input=GetComponent<PlayerInput>();
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
            cameraLookPoint=transform.Find("CameraLookPoint");
        }
        protected virtual void Update() 
        {
            movementStateMachine.HandleInut();
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
    }
}