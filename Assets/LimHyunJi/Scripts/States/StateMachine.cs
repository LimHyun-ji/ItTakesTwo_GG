using UnityEngine;

namespace ItTakesTwo
{
    public abstract class StateMachine 
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();

            currentState =newState;

            currentState.Enter();
        }
        public void HandleInut()
        {
            currentState?.HandleInput();
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
    }
}
