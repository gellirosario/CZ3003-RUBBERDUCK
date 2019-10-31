using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;

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
    public static int i = 1;

    void Start()
    {
        pullReport();
    }
    public void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
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
            FirebaseDatabase.DefaultInstance.GetReference("Report").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("HI, S =  what you have");
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot s = task.Result;

                    //for each world & stage
                    foreach (DataSnapshot node in s.Children)
                    {

                        int total = 0, correct = 0, wrong = 0;

                        //for each field in world & stage
                        foreach(DataSnapshot childNode in node.Children)
                        {
                            Debug.Log(childNode.Key + ": " + childNode.Value);

                            if (childNode.Key == "Correct")
                            {
                                correct = int.Parse(childNode.Value.ToString());
                                Debug.Log("Correct key found: " + correct);
                            }

                            if (childNode.Key == "Wrong")
                            {
                                wrong = int.Parse(childNode.Value.ToString());
                                Debug.Log("Wrong key found: " + wrong);
                            }
                        }

                        total = correct + wrong;
                        Debug.Log(total);

                        string concat = correct + "/" + total;
                        Debug.Log(concat);

                        switch (node.Key)
                        {
                            case "w1s1":
                                w1s1.text = concat;
                                break;
                            case "w1s2":
                                w1s2.text = concat;
                                break;
                            case "w1s3":
                                w1s3.text = concat;
                                break;
                            case "w2s1":
                                w2s1.text = concat;
                                break;
                            case "w2s2":
                                w2s2.text = concat;
                                break;
                            case "w2s3":
                                w2s3.text = concat;
                                break;
                            case "w3s1":
                                w3s1.text = concat;
                                break;
                            case "w3s2":
                                w3s2.text = concat;
                                break;
                            case "w3s3":
                                w3s3.text = concat;
                                break;
                            case "w4s1":
                                w4s1.text = concat;
                                break;
                            case "w4s2":
                                w4s2.text = concat;
                                break;
                            case "w4s3":
                                w4s3.text = concat;
                                break;
                            case "w5s1":
                                w5s1.text = concat;
                                break;
                            case "w5s2":
                                w5s2.text = concat;
                                break;
                            case "w5s3":
                                w5s3.text = concat;
                                break;
                        }


                    }

                }

            });

    }

}
