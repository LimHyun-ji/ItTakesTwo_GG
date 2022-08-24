using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class DummyScript : MonoBehaviour
    {
        Animator myAnimator;
        public Animator playerAnimator;
        // Start is called before the first frame update
        void Start()
        {
            myAnimator=GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //변수도 동적으로 바꿔줄 수 있는지
            myAnimator.runtimeAnimatorController=playerAnimator.runtimeAnimatorController;
        }
    }
}
