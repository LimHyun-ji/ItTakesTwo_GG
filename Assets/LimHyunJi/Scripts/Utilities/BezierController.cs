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
        
        public List<Vector3> pointsForEditor=new List<Vector3>();
        
        private void Start()
        {
            GetPoints(Points, points);
        }
        void Update()
        {
            //GetPosition();
        }

        private void FixedUpdate()
        {
            
        }

        //생성자
        public BezierController(List<GameObject> Points)
        {
            this.Points=Points;
            GetPoints(Points, points);
        }

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
    
            for(int i=0; i< n+1; i++)//0부터 n까지
            {
                //Debug.Log("Points" + points[i]);
                //계수
                float c=1;
                //계수 구하기
                for(int j=0; j< i; j++ )
                {
                    c *= (float)(n-j)/(float)(j+1);
                }
                movingPoint += c * points[i] * Mathf.Pow(value, i) * Mathf.Pow((1-value), n-i);
            }
            return movingPoint;
        }
        public void GetPoints(List<GameObject> Points, List<Vector3> points)
        {
            points.Clear();
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

}
