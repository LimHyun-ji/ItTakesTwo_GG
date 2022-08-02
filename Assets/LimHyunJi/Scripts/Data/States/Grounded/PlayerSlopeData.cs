using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [Serializable]
    public class PlayerSlopeData 
    {
        [field: SerializeField] [field: Range(0f,100f)] public float SlopeForce{get; private set;}=1f;
        [field: SerializeField] [field: Range(0f,10f)] public float slopeForceRayLength{get; private set;}=1.5f;
        [field: SerializeField] [field: Range(1f,2f)] public float speedModifier{get; private set;}=1f;


    }
}
