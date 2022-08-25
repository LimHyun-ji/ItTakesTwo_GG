using UnityEngine;
using System;

namespace ItTakesTwo
{
    [Serializable]
    public class PlayerGroundedData 
    {
        [field: SerializeField] [field: Range(0f, 25f)]  public float baseSpeed{get; private set;}=5f;
        [field: SerializeField] [field: Range(0f, 3f)]  public float groundCheckRadius{get; private set;}=2f;
        [field: SerializeField] [field: Range(0f, 5f)]  public float FlyCheckRadius{get; private set;}=4f;
        [field: SerializeField] [field: Range(0f, 3f)]  public float waterCheckRadius{get; private set;}=1f;
        [field: SerializeField] public AudioClip walkSound;
        [field: SerializeField] public AudioClip slideSound;
        [field: SerializeField] public AudioClip landSound;


        
        [field: SerializeField]  public PlayerRotationData BaseRotationData{get; private set;}
        [field: SerializeField]  public PlayerRunData RunData{get; private set;}
        [field: SerializeField]  public PlayerDashData DashData{get; private set;}
        [field: SerializeField]  public PlayerJumpData JumpData{get; private set;}
        [field: SerializeField]  public PlayerSlopeData SlopeData{get; private set;}


    }
}
