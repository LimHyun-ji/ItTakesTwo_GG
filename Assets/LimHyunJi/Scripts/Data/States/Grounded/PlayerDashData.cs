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
    [field: SerializeField] [field: Range(1f,10f)] public float DashDistance{get; private set;}=2f;
    [field: SerializeField]  public Vector3 DashDrag{get; private set;}=new Vector3(0.5f, 0.5f, 0.5f);
    [field: SerializeField] [field: Range(0f,2f)] public float DashTime{get; private set;}=10f;
    [field: SerializeField] [field: Range(1f,10f)] public float SprintTime{get; private set;}=10f;
    [HideInInspector] public int airDashCount=0;
    
    }
}
