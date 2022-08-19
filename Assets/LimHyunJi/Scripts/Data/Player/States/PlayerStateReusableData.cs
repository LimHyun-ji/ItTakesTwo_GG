using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerStateReusableData
    {
        public Vector2 MovementInput{get; set;}
        public bool isWallArea;
        public bool isGrounded;
        public float SpeedModifier {get; set;}=1f;
        private Vector3 currentTargetRotation;

        public ref Vector3 CurrentTargetRotation
        {
            get{ return ref currentTargetRotation;}
        }

        private Vector3 timeToReachTargetRotation;
        public ref Vector3 TimeToReachTargetRotation
        {
            get{ return ref timeToReachTargetRotation;}
        }

        private Vector3 dampedTargetRotationCurrentVelocity;
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get{ return ref dampedTargetRotationCurrentVelocity;}
        }

        private Vector3 dampedTargetRotationPassedTime;
        public ref Vector3 DampedTargetRotationPassedTime
        {
            get{ return ref dampedTargetRotationPassedTime;}
        }
    }
}
