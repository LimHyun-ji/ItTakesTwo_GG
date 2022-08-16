using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void LateUpdate();
        public void PhysicsUpdate();//FixedUpdate
        public void OnTriggerExit(Collider collider);
        public void OnTriggerEnter(Collider collider);
    }
}

