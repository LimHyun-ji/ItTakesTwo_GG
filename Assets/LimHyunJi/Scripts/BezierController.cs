using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ItTakesTwo
{
    public class BezierController : MonoBehaviour
    {
        public GameObject player;
        [Range(0, 1)]
        public float value;
        public float speed=2f;
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;
        public Vector3 P4;

        private void FixedUpdate()
        {
            value=Mathf.Lerp(value, 1, Time.deltaTime);
            player.transform.position = BezierTest(P1, P2, P3, P4, value);
            if(Vector3.Distance(player.transform.position, P4)<0.01)
            {
                Debug.Log("end");
                player.transform.position += (P4-P3).normalized * Time.deltaTime * speed*2f;
            }
            else
            {
                player.transform.position = BezierTest(P1, P2, P3, P4, value);
                player.transform.forward= (BezierTest(P1, P2, P3, P4, value+Time.deltaTime)- BezierTest(P1, P2, P3, P4, value)).normalized;
            }
        }
        public Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float value)
        {
            Vector3 A = Vector3.Lerp(P_1, P_2, value);
            Vector3 B = Vector3.Lerp(P_2, P_3, value);
            Vector3 C = Vector3.Lerp(P_3, P_4, value);

            Vector3 D = Vector3.Lerp(A, B, value);
            Vector3 E = Vector3.Lerp(B, C, value);

            Vector3 F = Vector3.Lerp(D, E, value);

            return F;
        }

    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BezierController))]
    public class BezierController_Editor : Editor
    {
        private void OnSceneGUI()
        {
            BezierController Generator = (BezierController)target;

            Generator.P1 = Handles.PositionHandle(Generator.P1, Quaternion.identity);
            Generator.P2 = Handles.PositionHandle(Generator.P2, Quaternion.identity);
            Generator.P3 = Handles.PositionHandle(Generator.P3, Quaternion.identity);
            Generator.P4 = Handles.PositionHandle(Generator.P4, Quaternion.identity);

            Handles.DrawLine(Generator.P1, Generator.P2);
            Handles.DrawLine(Generator.P3, Generator.P4);

            for(float i =0; i<50; i++)
            {
                float value_Before=i/50;
                Vector3 Before=Generator.BezierTest(Generator.P1, Generator.P2, Generator.P3, Generator.P4, value_Before);
                float value_After=(i+1)/50;
                Vector3 After= Generator.BezierTest(Generator.P1, Generator.P2, Generator.P3, Generator.P4, value_After);
                
                Handles.DrawLine(Before, After);
            }
        }
    }

}
