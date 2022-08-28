using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    //반드시 animator 스크립트와 같은 오브젝트에 있어야 함
    public class IKScript : MonoBehaviour
    {
        Animator animator;
        Player player;
        GameObject leftHand;
        GameObject rightHand;

        // Start is called before the first frame update
        void Start()
        {
            animator=GetComponent<Animator>();
            player=GetComponentInParent<Player>();
            leftHand = player.magnet.transform.Find("LeftHandPos").gameObject;
            rightHand =player.magnet.transform.Find("RightHandPos").gameObject;

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        
        private void OnAnimatorIK(int layerIndex)
        {
            if(player.isMagnet)
            {
                //손 IK
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.transform.position);
                //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                //animator.SetIKRotation(AvatarIKGoal.LeftHand, player.magnet.transform.rotation);//leftHand.transform.rotation);


                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.transform.position);
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                //animator.SetIKRotation(AvatarIKGoal.RightHand, player.magnet.transform.rotation);//rightHand.transform.rotation);
            }
        }
    }
}
