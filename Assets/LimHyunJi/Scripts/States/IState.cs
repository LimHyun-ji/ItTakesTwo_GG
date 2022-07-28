using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInut();
        public void Update();
        public void PhysicsUpdate();//FixedUpdate
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
        public void OnTriggerExit(Collider collider);
        public void OnTriggerEnter(Collider collider);
    }
}

