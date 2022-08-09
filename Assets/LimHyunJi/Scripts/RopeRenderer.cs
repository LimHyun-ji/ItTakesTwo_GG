using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class RopeRenderer : MonoBehaviour
    {
       LineRenderer rope;
       int count=10;
       void Start()
       {
            rope =GetComponent<LineRenderer>();
            rope.positionCount= count;
       }
       
       void Update()
       {
            
       }
       public void SetPosition (Vector3 startPos, Vector3 endPos)
       {
            float xInternal = (endPos.x -startPos.x)/count;
            float yInternal = (endPos.y -startPos.y)/count;
            float zInternal = (endPos.z -startPos.z)/count;

            for(int i =0; i < count; i++)
            {
                rope.SetPosition(i, startPos + new Vector3(xInternal, yInternal, zInternal)*i);
            }
       }
       
    }
}
