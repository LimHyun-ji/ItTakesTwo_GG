using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ItTakesTwo
{
    public class BezierController : MonoBehaviour
    {
        public GameObject player;
        [Range(1, 10)]
        public float speed=2f;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;
        public Vector3 p4;

        public Transform P1;
        public Transform P2;
        public Transform P3;
        public Transform P4;
        private bool isEnd;
        private bool flag;


        public List<GameObject> Points = new List<GameObject>();
        public List<Vector3> points=new List<Vector3>();
        
        private void Start()
        {
            GetPoints(Points, points);
        }
        void Update()
        {
            GetPosition();
        }

        private void FixedUpdate()
        {
            
        }

        //재귀로 구현하기
        // public Vector3 BezierTest(List<Vector3> pointList)
        // {
        //     //for(int i=0; )
        // } 

        //n차 베지어 곡선
        public Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float value)
        {
            Vector3 finalPoint;
            // Vector3 A = Vector3.Lerp(P_1, P_2, value);
            // Vector3 B = Vector3.Lerp(P_2, P_3, value);
            // Vector3 C = Vector3.Lerp(P_3, P_4, value);

            // Vector3 D = Vector3.Lerp(A, B, value);
            // Vector3 E = Vector3.Lerp(B, C, value);

            // Vector3 F = Vector3.Lerp(D, E, value);

            // return F;


            finalPoint=P_1* Mathf.Pow((1- value), 3) 
                    + 3* P_2* Mathf.Pow((value), 1) * Mathf.Pow((1- value), 2)
                    + 3 * P_3* Mathf.Pow((value), 2) * Mathf.Pow((1- value), 1)
                    + P_4* Mathf.Pow((value), 3);

            return finalPoint;
        }

        public Vector3 BezierTest(List<Vector3> points, float value, int n)
        {
            Vector3 movingPoint=Vector3.zero;
            if(!flag)
            {
                
            }
            for(int i=0; i< n+1; i++)//0부터 n까지
            {
                Debug.Log("Count");
                //계수
                int c=1;
                //계수 구하기
                for(int j=0; j< i; j++ )
                {
                    c *= (n-j)/(j+1);
                }
                movingPoint += points[i] * c * Mathf.Pow(value, i) * Mathf.Pow((1-value), n-i);
            }
            return movingPoint;
        }
        public void GetPoints(List<GameObject> Points, List<Vector3> points)
        {
            for(int i=0; i< Points.Count; i++)
            {   
                points.Add(Points[i].transform.position);
            }
        }
        public void GetPosition()
        {
            p1 = P1.position;
            p2 = P2.position;
            p3 = P3.position;
            p4 = P4.position;
        }

        //3차 베지어 곡선을 Points 점들로 찍기 4차!!점 4개
        public Vector3 MakeBezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float value)
        {
            Vector3 finalPoint=P_1* Mathf.Pow((1- value), 3) //0
                    + 3* P_2* Mathf.Pow((value), 1) * Mathf.Pow((1- value), 2)//1
                    + 3 * P_3* Mathf.Pow((value), 2) * Mathf.Pow((1- value), 1)//2
                    + P_4* Mathf.Pow((value), 3); //3

            return finalPoint;
        }

    }


    //Editor에서 편집하기
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BezierController))]
    public class BezierController_Editor : Editor
    {
        private void OnSceneGUI()
        {
            BezierController Generator = (BezierController)target;

            Generator.p1 = Generator.P1.position;// Handles.PositionHandle(Generator.p1, Quaternion.identity);
            Generator.p2 = Generator.P2.position;// Handles.PositionHandle(Generator.p2, Quaternion.identity);
            Generator.p3 = Generator.P3.position;// Handles.PositionHandle(Generator.p3, Quaternion.identity);
            Generator.p4 = Generator.P4.position;// Handles.PositionHandle(Generator.p4, Quaternion.identity);

            Handles.DrawLine(Generator.p1, Generator.p2);
            Handles.DrawLine(Generator.p3, Generator.p4);

            for(float i =0; i<50; i++)
            {
                float value_Before=i/50;
                Vector3 Before=Generator.BezierTest(Generator.p1, Generator.p2, Generator.p3, Generator.p4, value_Before);
                float value_After=(i+1)/50;
                Vector3 After= Generator.BezierTest(Generator.p1, Generator.p2, Generator.p3, Generator.p4, value_After);
                
                Handles.DrawLine(Before, After);
            }
        }
    }

}
