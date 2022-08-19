using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerForceDownState : PlayerAirborneState
    {
        PlayerForceDownData forceDownData; 
        private float currentTime;
        CameraController camera;

        public PlayerForceDownState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            forceDownData= stateMachine.Player.Data.ForceDownData;
        }

        public override void Enter()
        {
            base.Enter();
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();
            stateMachine.Player.animator.SetTrigger("ForceDown");

            stateMachine.Player.isMovable=false;
            stateMachine.Player.velocity.y += 10f;
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
            
            camera.Play();
            stateMachine.Player.isMovable=true;
            currentTime=0f;
        }

    }
}
