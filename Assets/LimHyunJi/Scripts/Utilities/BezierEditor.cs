// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// namespace ItTakesTwo
// {
//     //에디터에서 씬 보면서할 때만 활성화
//     //나머지는 비활성화
//     public class BezierEditor : MonoBehaviour
//     {
//         //어디 스크립트에 붙이지 않아도 항상 실행중
//         //Editor에서 편집하기
//         [CanEditMultipleObjects]
//         [CustomEditor(typeof(BezierController))]
//         public class BezierController_Editor : Editor
//         {
//             bool flag=false;
//             private void OnSceneGUI()
//             {
//                 BezierController Generator = (BezierController)target;

//                 // Generator.p1 = Generator.P1.position;// Handles.PositionHandle(Generator.p1, Quaternion.identity);
//                 // Generator.p2 = Generator.P2.position;// Handles.PositionHandle(Generator.p2, Quaternion.identity);
//                 // Generator.p3 = Generator.P3.position;// Handles.PositionHandle(Generator.p3, Quaternion.identity);
//                 // Generator.p4 = Generator.P4.position;// Handles.PositionHandle(Generator.p4, Quaternion.identity);
//                 if(!flag)
//                 {
//                     flag=true;
//                     Generator.GetPoints(Generator.Points, Generator.pointsForEditor);
//                 }

//                 //UpdatePoints
//                 for(int i=0; i< Generator.pointsForEditor.Count; i++)
//                 {
//                     Generator.pointsForEditor[i]= Generator.Points[i].transform.position;
//                 }
//                 //Draw Points
//                 for(int i=0; i< Generator.pointsForEditor.Count-1; i+=2)
//                 {
//                     Handles.DrawLine(Generator.pointsForEditor[i], Generator.pointsForEditor[i+1]);
//                 }
//                 // Handles.DrawLine(Generator.p1, Generator.p2);
//                 // Handles.DrawLine(Generator.p3, Generator.p4);

//                 for(float i =0; i<50; i++)
//                 {
//                     float value_Before=i/50;
//                     Vector3 Before=Generator.BezierTest(Generator.pointsForEditor, value_Before, Generator.pointsForEditor.Count-1);
//                     float value_After=(i+1)/50;
//                     Vector3 After= Generator.BezierTest(Generator.pointsForEditor, value_After, Generator.pointsForEditor.Count-1);
                    
//                     Handles.DrawLine(Before, After);
//                 }
//             }
//         }
//     }
// }
