/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class RopeRider : MonoBehaviour
    {
        List<GameObject> Points; 
        // Start is called before the first frame update
        void Start()
        {
            for(int i=0; i<GameObject.Find("BezierController2").transform.childCount; i++)
            {
                Points.Add(GameObject.Find("BezierController2").transform.GetChild(i).gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void MakeRope()
        {
            //BezierController Generator=new BezierController(Points)
            for(float i =0; i<50; i++)
            {
                float value_Before=i/50;
                Vector3 Before=Generator.BezierTest(Generator.pointsForEditor, value_Before, Generator.pointsForEditor.Count-1);
                float value_After=(i+1)/50;
                Vector3 After= Generator.BezierTest(Generator.pointsForEditor, value_After, Generator.pointsForEditor.Count-1);
            
                Handles.DrawLine(Before, After);
            }
            
        }
    }
}
*/
