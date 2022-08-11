using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [Serializable]
    public class PlayerWallData
    {
        [field: SerializeField] [field: Range(0f,100f)] public float SpeedModifier{get; private set;}=2f;
         [field: SerializeField] [field: Range(0f,10f)] public float wallIdleTime{get; private set;}=2f;
        [HideInInspector] public int wallJumpCount=0;

    }
}
