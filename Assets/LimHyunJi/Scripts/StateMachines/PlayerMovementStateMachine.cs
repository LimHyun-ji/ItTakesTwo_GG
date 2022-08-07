using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo

{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player Player{get;}
        public PlayerStateReusableData ReusableData{get;}
        public PlayerIdlingState IdlingState {get;}
        public PlayerRunningState RunningState {get;}
        public PlayerSprintingState SprintingState {get;}
        public PlayerDashingState DashingState {get;}
        public PlayerJumpingState JumpingState{get;} 
        public PlayerFallingState FallingState{get;}
        public PlayerForceDownState ForceDownState{get;}
        public PlayerSlidingState SlidingState{get;}
        public PlayerSwingState SwingState {get;}


        public PlayerMovementStateMachine(Player player)//생성자
        {
            Player=player;
            ReusableData=new PlayerStateReusableData();
            IdlingState=new PlayerIdlingState(this);
            RunningState=new PlayerRunningState(this);
            SprintingState=new PlayerSprintingState(this);
            DashingState =new PlayerDashingState(this);
            JumpingState=new PlayerJumpingState(this);
            FallingState=new PlayerFallingState(this);
            ForceDownState=new PlayerForceDownState(this);
            SlidingState = new PlayerSlidingState(this);
            SwingState = new PlayerSwingState(this);
        }
    }
}

