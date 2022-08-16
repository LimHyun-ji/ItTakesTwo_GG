using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CameraMovementStateMachine : StateMachine
    {
        public CameraController Camera{get;}

        public CameraMovementStateMachine(CameraController camera)
        {
            Camera =camera;
        }
    }
}
