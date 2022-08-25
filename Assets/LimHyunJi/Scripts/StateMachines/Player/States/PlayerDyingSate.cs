using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerDyingSate : PlayerBaseState
    {
        PlayerDieData dieData;
        private float currentTime=0f;
        public PlayerDyingSate(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.interactableObject=null;
            dieData=stateMachine.Player.Data.DieData;
            stateMachine.Player.model.SetActive(false);
            camera.otherDummy.SetActive(false);
            stateMachine.Player.magnet.SetActive(false);

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            currentTime +=Time.deltaTime;
            if(currentTime > dieData.SaveDelayTime)
            {
                stateMachine.Player.model.SetActive(true);
                camera.otherDummy.SetActive(true);
                stateMachine.Player.magnet.SetActive(true);
                GoToSavePoint();
                stateMachine.ChangeState(stateMachine.IdlingState);
                currentTime=0f;
            }
        }
        public override void OnTriggerEnter(Collider collider)
        {
        }
        public override void OnTriggerExit(Collider collider)
        {
        }
    }
}
