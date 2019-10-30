using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ReportController : MonoBehaviour
{
    public Text w1s1;

    void Start()
    {
        w1s1.text = "TEST";
    }

    public void pullReport()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Report").Child("w1s1").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("HI, S =  waht you have");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot s = task.Result;
                foreach (DataSnapshot node in s.Children)
                {
                    Debug.Log(node.Key + ": " + node.Value);

                    if (node.Key == "Correct")
                    {
                       int correctAns =  int.Parse(node.Value.ToString());

                    }

                    if (node.Key == "Wrong")
                    {
                       int wrongAns = int.Parse(node.Value.ToString());
                    }
                }
                // updateReport(correctAns, wrongAns);
            }
        });

    }
}
