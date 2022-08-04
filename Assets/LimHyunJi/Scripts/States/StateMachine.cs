using UnityEngine;

namespace ItTakesTwo
{
    public abstract class StateMachine 
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();   //점프 중에 또 점프 안하려고

            currentState =newState;

            currentState.Enter();
        }
        public void HandleInut()
        {
            currentState?.HandleInut();
        }
        public void Update()
        {
            currentState?.Update();
        }
        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }
        public void OnTriggerEnter(Collider collider)
        {
            currentState?.OnTriggerEnter(collider);
        }

        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
        }
        public void OnAnimationEnterEvent()
        {
            currentState?.OnAnimationEnterEvent();
        }
        public void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }
           public void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }
    }
}
