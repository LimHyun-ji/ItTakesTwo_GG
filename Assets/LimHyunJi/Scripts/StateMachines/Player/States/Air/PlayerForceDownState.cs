using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerForceDownState : PlayerAirborneState
    {
        PlayerForceDownData forceDownData; 
        private float currentTime;

        public PlayerForceDownState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            forceDownData= stateMachine.Player.Data.ForceDownData;
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetTrigger("ForceDown");

            stateMachine.Player.isMovable=false;
            stateMachine.Player.velocity.y += 10f;
            stateMachine.Player.isForceDown=true;
        }
        public override void PhysicsUpdate()
        {
            currentTime += Time.deltaTime;
            if(currentTime> forceDownData.ForceDownDelayTime)
            {
                UseGravity(forceDownData.GravityScale);
            }
            //충돌한 물체, 버튼 누르기
        }
        
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.isForceDown=false;
            
            camera.Play();
            stateMachine.Player.isMovable=true;
            currentTime=0f;
        }

    }
}
