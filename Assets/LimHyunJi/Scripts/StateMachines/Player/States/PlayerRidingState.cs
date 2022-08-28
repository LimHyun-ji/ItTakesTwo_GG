using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayerRidingState : PlayerBaseState
    {
        private GameObject bezierObj;
        BezierController bezierController;
        List<GameObject> PointList= new List<GameObject>();
        float value=0;
        public PlayerRidingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.animator.SetBool("IsRiding", true);
            stateMachine.Player.animator.SetTrigger("Riding");
            stateMachine.Player.velocity.y=-1f;
            camera=stateMachine.Player.mainCameraTransform.gameObject.GetComponent<CameraController>();
            camera.currentState=CameraController.CameraState.RidingState;

            movementData.JumpData.airJumpCount=0;

            // bezierController = stateMachine.Player.interactableObject.GetComponent<BezierController>();
            // bezierController.enabled=true;

            // bezierController.player = stateMachine.Player.gameObject;

            //자식들의 pOISITON LIST  넘겨주기
            GetPointObjList();
            bezierController =new BezierController(PointList);
            
            stateMachine.Player.interactableObject=null;
        }
        public override void PhysicsUpdate()
        {
            value += Time.deltaTime / Mathf.Sqrt(bezierController.points.Count);//Mathf.Lerp(value, 1, Time.deltaTime);//
            if(value>1)
            {
                OnFall();
            }

            stateMachine.Player.transform.position =// bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value);
            bezierController.BezierTest(bezierController.points, value,bezierController.points.Count-1 );
            stateMachine.Player.transform.forward= (bezierController.BezierTest(bezierController.points, value+Time.deltaTime, bezierController.points.Count-1)- bezierController.BezierTest(bezierController.points, value, bezierController.points.Count-1)).normalized;
            //(bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value+Time.deltaTime)- bezierController.BezierTest(bezierController.p1,bezierController.p2, bezierController.p3, bezierController.p4, value)).normalized
            if(Vector3.Distance(stateMachine.Player.transform.position, bezierController.points[bezierController.points.Count-1])<0.1)
            {
                stateMachine.Player.transform.eulerAngles=new Vector3(0, stateMachine.Player.transform.eulerAngles.y, stateMachine.Player.transform.eulerAngles.z);
                OnFall();
            }
        }
        
        public override void Exit()
        {
            base.Exit();

            stateMachine.Player.animator.SetBool("IsRiding", false);
            stateMachine.Player.animator.ResetTrigger("Riding");


            //bezierController.enabled=false;
            //stateMachine.Player.characterController.Move(stateMachine.Player.transform.forward * 3f);
            camera.currentState=CameraController.CameraState.IdleState;
        }

        //interactable Object 베지어 곡선 자식들을 베지어 스크립트 Points로 넣기
        private void GetPointObjList()
        {
            PointList.Clear();
            for(int i=0; i< stateMachine.Player.interactableObject.transform.childCount; ++i)
            {
                PointList.Add(stateMachine.Player.interactableObject.transform.GetChild(i).gameObject);
            }
        }
        
    }
}
