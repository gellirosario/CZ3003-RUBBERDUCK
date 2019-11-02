using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class AssignmentController : MonoBehaviour
{
    public static AssignmentController Instance { get; set; }

    public InputField createAssignmentInput;
    public InputField deleteAssignmentInput;
    public Text displayDeleteMessage;
    public string uid;
    private List<Question> questionList = new List<Question>();
    private List<Assignment> assignmentList = new List<Assignment>();
    public Assignment assignmentData { get; private set; }
    private DatabaseReference reference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set up the Editor before calling into the realtime database.
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");

                // Get the root reference location of the database.
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        // Retrieve Question List According to World and Stage
        print("Before loading questionlist count: " + questionList.Count);
        if (questionList.Count == 0)
        {
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1, 1);
        }
    }



    public void SaveAssignment()
    {
        print("After loading questionlist count: " + questionList.Count);
        //DatabaseReference referenceRef = reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(createAssignmentInput.text);

        for (int i = 0; i < questionList.Count; i++)
        {
            Assignment assignment = new Assignment(createAssignmentInput.text, questionList[i].qnID, questionList[i].world, questionList[i].stage, questionList[i].difficulty,
                questionList[i].question, questionList[i].answer, questionList[i].option1, questionList[i].option2, questionList[i].option3, questionList[i].option4);
            string json = JsonUtility.ToJson(assignment);
            reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(assignment.assignmentName).SetRawJsonValueAsync(json);
            //referenceRef.Child((i+1).ToString()).SetRawJsonValueAsync(json);
        }
        createAssignmentInput.text = "";
    }

    public void LoadAssignment()
    {
        Debug.LogFormat("----HERE---");
        uid = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----ASSIGNMENT INFO ID---" + uid);

        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Assignment table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot assignmentNode in snapshot.Children)
                {
                    Debug.Log("USER ID OF FIREBASE:" + assignmentNode.Key);
                    Debug.Log("USER ID OF CURRENT USER:" + uid);
                    if (assignmentNode.Key == uid)
                    {
                        //load assignment data into assignment object
                        var assignmentDict = (IDictionary<string, object>)assignmentNode.Value;
                        print("assignment dictionary count: " + assignmentDict.Count);

                        foreach (KeyValuePair<string, object> kvp in assignmentDict)
                        {
                            Debug.LogFormat("ASSIGNMENT ---- Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                            var assignmentNameDict = (IDictionary<string, object>)assignmentDict[kvp.Key];

                            foreach (KeyValuePair<string, object> kvp1 in assignmentNameDict)
                            {
                                Debug.LogFormat("ASSIGNMENT NAME ---- Key = {0}, Value = {1}", kvp1.Key, kvp1.Value);
                            }

                            assignmentData = new Assignment(assignmentNameDict);
                            Debug.Log("Assignment Name: " + assignmentData.assignmentName);
                            assignmentList.Add(assignmentData);
                            print("Assignment list count: " + assignmentList.Count);
                        }
                    }
                }
            }

            print("Load firebase assignment list count in controller: " + assignmentList.Count);
            PlayerPrefs.SetInt("ALCount", assignmentList.Count);
            for (var i = 0; i < assignmentList.Count; i++)
            {
                PlayerPrefs.SetString("AssignmentList(assignmentName)" + i, assignmentList[i].assignmentName);
                PlayerPrefs.SetInt("AssignmentList(qnID)" + i, assignmentList[i].qnID);
                PlayerPrefs.SetInt("AssignmentList(world)" + i, assignmentList[i].world);
                PlayerPrefs.SetInt("AssignmentList(stage)" + i, assignmentList[i].stage);
                PlayerPrefs.SetString("AssignmentList(difficulty)" + i, assignmentList[i].difficulty);
                PlayerPrefs.SetString("AssignmentList(question)" + i, assignmentList[i].question);
                PlayerPrefs.SetInt("AssignmentList(answer)" + i, assignmentList[i].answer);
                PlayerPrefs.SetString("AssignmentList(option1)" + i, assignmentList[i].option1);
                PlayerPrefs.SetString("AssignmentList(option2)" + i, assignmentList[i].option2);
                PlayerPrefs.SetString("AssignmentList(option3)" + i, assignmentList[i].option3);
                PlayerPrefs.SetString("AssignmentList(option4)" + i, assignmentList[i].option4);
            }
        });
    }


    public void DeleteAssignment()
    {
        bool assignmentNameFound = false;
        Debug.LogFormat("----HERE---");
        uid = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----ASSIGNMENT INFO ID---" + uid);

        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Assignment table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot assignmentNode in snapshot.Children)
                {
                    Debug.Log("USER ID OF FIREBASE:" + assignmentNode.Key);
                    Debug.Log("USER ID OF CURRENT USER:" + uid);
                    if (assignmentNode.Key == uid)
                    {
                        var assignmentDict = (IDictionary<string, object>)assignmentNode.Value;
                        foreach (var key in assignmentDict.Keys) // loop through keys
                        {
                            Debug.Log("ASSIGNMENT NAME:" + key);
                            Debug.Log("Inputted Text:" + deleteAssignmentInput.text);
                            if (key.ToString() == deleteAssignmentInput.text.ToString())
                            {
                                reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(deleteAssignmentInput.text).RemoveValueAsync();
                                displayDeleteMessage.text = deleteAssignmentInput.text + "\n Successfully Deleted";
                                assignmentNameFound = true;
                                break;
                            }
                        }

                    }
                }
            }
        });

        if (!assignmentNameFound)
        {
            displayDeleteMessage.text = deleteAssignmentInput.text + "\n Is Not Found In The Database";
        }

        Invoke("ClearMessage", 3);
    }

    void ClearMessage()
    {
        deleteAssignmentInput.text = "";
        displayDeleteMessage.text = "";
    }
}
