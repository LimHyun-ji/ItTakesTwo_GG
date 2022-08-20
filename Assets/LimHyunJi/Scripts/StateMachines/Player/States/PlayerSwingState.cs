using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerSwingState : PlayerBaseState
    {
        Vector3 initEulerAngles;
        Vector3 initLocalPos;
        Vector3 targetLocalPos;
        RopeRenderer ropeRenderer;
        LineRenderer lineRenderer;
        GameObject interactableObject;
        public PlayerSwingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Update()
        {
            //base.Update();
            //MoveLocalPos(initLocalPos,targetLocalPos); 
        }
        public override void PhysicsUpdate()
        {
            InputSwing();
            DefaultSwing();
            MakeRope();
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.velocity.y=-1f;

            stateMachine.Player.animator.SetTrigger("Swing");
            stateMachine.Player.animator.SetBool("IsSwing", true);

            interactableObject=stateMachine.Player.interactableObject;
            
            GameObject rope= GameObject.FindGameObjectWithTag("Rope");
            ropeRenderer= rope.GetComponent<RopeRenderer>();
            lineRenderer =rope.GetComponent<LineRenderer>();
            lineRenderer.enabled=true;
            stateMachine.Player.characterController.enabled=false;

            movementData.JumpData.airJumpCount=0;
            initEulerAngles=interactableObject.transform.eulerAngles;
            stateMachine.Player.transform.forward=interactableObject.transform.forward;
            stateMachine.Player.gameObject.transform.SetParent(interactableObject.transform);

            initLocalPos= stateMachine.Player.transform.localPosition;
            targetLocalPos = TargetLocalPos(7f);

            ResetLocalTransform(7f);
        }

        public override void Exit()
        {
            base.Exit();

            stateMachine.Player.animator.ResetTrigger("Swing");
            stateMachine.Player.animator.SetBool("IsSwing", false);

            stateMachine.Player.gameObject.transform.SetParent(null);
            stateMachine.Player.characterController.enabled=true;
            stateMachine.Player.characterController.Move(interactableObject.transform.forward* 7f);
            lineRenderer.enabled=false;
        }

        private void ResetLocalTransform(float distance)
        {
            stateMachine.Player.transform.localPosition= new Vector3(0, -distance, 0);
            stateMachine.Player.transform.localEulerAngles= Vector3.zero;
            stateMachine.Player.transform.localScale=Vector3.one;
        }

        private Vector3 TargetLocalPos(float distance)
        {
            return new Vector3(0, -distance, 0);
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
                xEulerAngle=Mathf.Lerp(xEulerAngle,-3f, Time.deltaTime);//3f는 이동할 각도만큼
                if(x< -30)    isEnd=true;
            }
            if(isEnd)//뒤로
            {
                xEulerAngle=Mathf.Lerp(xEulerAngle,3f, Time.deltaTime);
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


        private void MoveLocalPos(Vector3 initPos, Vector3 targetPos)
        {
            initPos= Vector3.Lerp(initPos, targetPos, Time.deltaTime);
            stateMachine.Player.transform.localPosition=initPos;
        }
        private void MakeRope()
        {
            ropeRenderer.SetPosition(interactableObject.transform.position, stateMachine.Player.transform.position);
        }
    }
}
