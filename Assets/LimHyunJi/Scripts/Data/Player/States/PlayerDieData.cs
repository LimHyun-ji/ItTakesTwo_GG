using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [Serializable]
    public class PlayerDieData
    {
        [field: SerializeField] [field: Range(0f,-100f)] public float DieVelocityY=-20f;
        [field: SerializeField] [field: Range(0f,10f)] public float SaveDelayTime=1f;
    }
}
