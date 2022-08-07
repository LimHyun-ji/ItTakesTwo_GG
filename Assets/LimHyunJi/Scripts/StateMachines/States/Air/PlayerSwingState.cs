using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerSwingState : PlayerMovementState
    {
        Vector3 initEulerAngles;
        public PlayerSwingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void PhysicsUpdate()
        {
            //interactableObject.transform.forward=stateMachine.Player.transform.forward;
            InputSwing();
            DefaultSwing();
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("interactable : " +interactableObject);
            initEulerAngles=interactableObject.transform.eulerAngles;
            stateMachine.Player.transform.forward=interactableObject.transform.forward;
            stateMachine.Player.gameObject.transform.SetParent(interactableObject.transform, false);
            ResetLocalTransform(3f);
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.gameObject.transform.SetParent(null);
            stateMachine.Player.characterController.Move(interactableObject.transform.forward* 5f);
        }


        private void ResetLocalTransform(float distance)
        {
            stateMachine.Player.transform.localPosition= new Vector3(0, -distance, 0);
            stateMachine.Player.transform.localEulerAngles= Vector3.zero;
            stateMachine.Player.transform.localScale=Vector3.one;
        }
        float xEulerAngle;
        bool isEnd;

        private void DefaultSwing()
        {
            var x=interactableObject.transform.eulerAngles.x;
            if(x>180)
                x-=360;
            if(!isEnd)//앞으로
            {
                xEulerAngle=Mathf.Lerp(xEulerAngle,-3f, Time.deltaTime);
                if(x< -30)    isEnd=true;
            }
            if(isEnd)//뒤로
            {
                xEulerAngle=Mathf.Lerp(xEulerAngle,3f, Time.deltaTime);
                //xEulerAngle=Mathf.Lerp(, -30, Time.deltaTime);
                if(x>30)    isEnd=false;
            }
            interactableObject.transform.Rotate(interactableObject.transform.right, xEulerAngle, Space.World); 
        }
        float x;
        float y;
        private void InputSwing()
        {
            x =  stateMachine.ReusableData.MovementInput.x;
            y =  stateMachine.ReusableData.MovementInput.y;
            if(y ==0f)
            {
                var z=interactableObject.transform.eulerAngles.z;
                z=ResetEulerAngleAxis(z, 0f);

                var y =interactableObject.transform.eulerAngles.y;
                y=ResetEulerAngleAxis(y, initEulerAngles.y);
                interactableObject.transform.eulerAngles=new Vector3(interactableObject.transform.eulerAngles.x, y, z);
            }

            interactableObject.transform.Rotate(stateMachine.Player.transform.forward, x, Space.World); 
            interactableObject.transform.Rotate(interactableObject.transform.right, -y, Space.World); 
        }
        private float ResetEulerAngleAxis(float a, float targetAngle)
        {
            if(a>180)
                a-=360;
            a=Mathf.Lerp(a, targetAngle, Time.deltaTime);
            return a;
        }
    }
}
