using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{       
    [Serializable]

    public class PlayerJumpData 
    {
        [field: SerializeField] [field: Range(0f,100f)] public float JumpHeight{get; private set;}=2f;
        [HideInInspector] public int airJumpCount=0;
    }
}
