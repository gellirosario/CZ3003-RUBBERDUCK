using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
public class AssignmentController : MonoBehaviour
{
    public InputField createAssignmentInput;
    public InputField deleteAssignmentInput;
    public Text displayDeleteMessage;
    public string id;
    private List<Question> questionList = new List<Question>();
    private List<Assignment> assignmentList = new List<Assignment>();
    public Assignment assignmentData { get; private set; }
    private DatabaseReference reference;

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
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1,1);
        }
    }

    public void SaveAssignment()
    {
        print("After loading questionlist count: " + questionList.Count);
        for (int i = 0; i < questionList.Count; i++)
        {
            Assignment assignment = new Assignment(createAssignmentInput.text, questionList[i].qnID, questionList[i].world, questionList[i].stage, questionList[i].difficulty,
                questionList[i].question, questionList[i].answer, questionList[i].option1, questionList[i].option2, questionList[i].option3, questionList[i].option4);
            string json = JsonUtility.ToJson(assignment);
            //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).SetRawJsonValueAsync(json);
            reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(assignment.assignmentName).SetRawJsonValueAsync(json);
        }
    }

    public List<Assignment> LoadAssignment()
    {
        Debug.LogFormat("----HERE---");
        id = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----ASSIGNMENT INFO ID---" + id);

        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWith(task =>
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
                    print("User ID: " + assignmentNode.Key);
                    print("User ID: " + id);
                    if (assignmentNode.Key == id)
                    {
                        //load assignment data into assignment object
                        var assignmentDict = (IDictionary<string, object>)assignmentNode.Value;
                        print("assignment dictionary count: " + assignmentDict.Count);
                        print("assignment dicitonary contents: " + assignmentDict.ToString());

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
            for (int i = 0; i < assignmentList.Count; i++)
            {
                print((i+1) + " loop");
                print(assignmentList[i].assignmentName + assignmentList[i].qnID + assignmentList[i].world + assignmentList[i].stage + assignmentList[i].difficulty + assignmentList[i].question +
                    assignmentList[i].answer + assignmentList[i].option1 + assignmentList[i].option2 + assignmentList[i].option3 + assignmentList[i].option4);
            }
        });
        return assignmentList;
    }

    public void DeleteAssignment()
    {
        reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(deleteAssignmentInput.text).RemoveValueAsync();
        displayDeleteMessage.text = deleteAssignmentInput.text + " Successfully Deleted" ;
    }
}
