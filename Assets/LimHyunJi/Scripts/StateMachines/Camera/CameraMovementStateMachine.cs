using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CameraMovementStateMachine : StateMachine
    {
        public CameraController Camera{get;}
        public CameraStateReusableData ReusableData{get;}
        public CameraBaseState BaseState{get;}
        public CameraMovementStateMachine(CameraController camera)
        {
            Camera =camera;
            ReusableData=new CameraStateReusableData();
            BaseState  =new CameraBaseState(this);
        }
    }
}
