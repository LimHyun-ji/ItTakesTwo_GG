using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ItTakesTwo
{
    public class DialogManager : MonoBehaviour
    {
        Text dialog;
        List<string> stringList=new List<string>();
        int count=0;
        void Start()
        {
            dialog=GameObject.Find("DialogBox").GetComponent<Text>();
            string filePath=Path.Combine(Application.streamingAssetsPath, "Dialog_Hakim.csv");
            Debug.Log(ReadTxt(filePath));
            
            //StartCoroutine(ShowDialog(stringList));
        }
        private void Update() {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                ShowDialogEvent();
            }
        }
        string ReadTxt(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string value;
            if(fileInfo.Exists)
            {
                StreamReader reader= new StreamReader(filePath);

                while(reader.Peek() != -1)
                {
                    stringList.Add(reader.ReadLine());
                }
                value=reader.ReadToEnd();
                reader.Close();
            }
            else   
                value="파일이 없습니다.";
            return value;
        }

        private IEnumerator ShowDialog(List<string> stringList)
        {
            for(int i=0; i< stringList.Count; i++)
            {
                dialog.text=stringList[i];
                yield return new WaitForSeconds(1.5f);
            }
        }
        public void ShowDialogEvent()
        {
            dialog.text=stringList[count];
            count++;
        }
        
        


    }
}
