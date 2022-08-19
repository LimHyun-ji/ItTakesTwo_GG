using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    [CreateAssetMenu(fileName = "Player", menuName ="Custom/Character/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundedData GroundedData{get; private set;}
        [field: SerializeField] public PlayerForceDownData ForceDownData{get; private set;}
        [field: SerializeField] public PlayerWallData wallData{get; private set;}

    }
}
