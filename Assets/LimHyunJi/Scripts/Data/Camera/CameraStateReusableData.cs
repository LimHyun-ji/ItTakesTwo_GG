using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class CameraStateReusableData
    {
        public float smoothSpeed=10f;
        public float mouseSpeed=10f;
        public float cameraObstacleSpeed=10f;
        public float minDistance=3f;
        public float baseDistance =20f;
        public float maxDistance =30f;
        private Vector3 offset;
        private bool isObstacle;
        private Vector3 fixedPoint;
        public Vector2 mouseInput;
        private Vector3 velocity = Vector3.zero;
    }
}
