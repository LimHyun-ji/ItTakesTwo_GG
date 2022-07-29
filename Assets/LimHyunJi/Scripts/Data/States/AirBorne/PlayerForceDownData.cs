using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [Serializable]
    public class PlayerForceDownData 
    {
        [field: SerializeField] [field: Range(500f,1000f)] public float GravityScale{get; private set;}=1000f;
        [field: SerializeField] [field: Range(0f,10f)] public float ForceDownDelayTime=2f;
    }
}
