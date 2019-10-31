using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ReportController : MonoBehaviour
{
    private DatabaseReference reference;

    public static int w1s1R;
    public Report reportData { get; private set; }


    public Text w1s1, w1s2, w1s3;
    public Text w2s1, w2s2, w2s3;
    public Text w3s1, w3s2, w3s3;
    public Text w4s1, w4s2, w4s3;
    public Text w5s1, w5s2, w5s3;
    private string result1;
    private string worldAndStage;

    public static List<Report> playerName = new List<Report>();

    void Start()
    {
        // pullReport();
       // w1s1.text = w1Report.w1s1r.ToString();
    }
    public void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void pullReport()
    {
        int i=1, y=1;

       // for (i = 1; i <= 5; i++)
      //  {
            
           // for(y=1;y<=3;y++)
          //  {
                worldAndStage = "w" + i + "s" + y;
                //Debug.Log(worldAndStage);
                FirebaseDatabase.DefaultInstance.GetReference("Report").Child(worldAndStage).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("HI, S =  what you have");
                        // Handle the error...
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot s = task.Result;
                        foreach (DataSnapshot node in s.Children)
                        {
                            Debug.Log(node.Key + ": " + node.Value);

                            if (node.Key == "Appear")
                            {
                              // w1Report.w1s1r = int.Parse(node.Value.ToString());
                               // report.w1s1r = int.Parse(node.Value.ToString());

                                //correctAns11 += int.Parse(node.Value.ToString());
                            }

                        }
                
                    }
                    

                });

          //  } // end of iner for loop
       // } // end of out for loop
       

    }
   
}
