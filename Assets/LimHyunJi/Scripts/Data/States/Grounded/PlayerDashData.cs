using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [Serializable]

    public class PlayerDashData 
    {
       
    [field: SerializeField] [field: Range(1f,10f)] public float speedModifier{get; private set;}=10f;
    [field: SerializeField] [field: Range(1f,10f)] public float DashTime{get; private set;}=2f;
    [field: SerializeField] [field: Range(1f,10f)] public float SprintTime{get; private set;}=10f;
    }
}
