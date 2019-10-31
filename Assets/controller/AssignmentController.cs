using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
public class AssignmentController : MonoBehaviour
{
    public InputField assignmentInput;
    public static string assignmentName;
    public string id;
    private List<Question> questionList = new List<Question>();
    private List<Assignment> assignmentList = new List<Assignment>();
    public Assignment assignmentData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve Question List According to World and Stage
        print("Before loading questionlist count: " + questionList.Count);
        if (questionList.Count == 0)
        {
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1,1);
        }
    }

    public void SaveAssignment()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        //Assignment assignment = new Assignment(assignmentInput.text,0,0,0,"1","1",0,"1", "1", "1", "1");
        //string json = JsonUtility.ToJson(assignment);

        //reference.Child("Assignment").SetRawJsonValueAsync(json);
        print("After loading questionlist count: " + questionList.Count);
        for (int i = 0; i < questionList.Count; i++)
        {
            Assignment assignment = new Assignment(assignmentInput.text, questionList[i].qnID, questionList[i].world, questionList[i].stage, questionList[i].difficulty,
                questionList[i].question, questionList[i].answer, questionList[i].option1, questionList[i].option2, questionList[i].option3, questionList[i].option4);
            string json = JsonUtility.ToJson(assignment);
            //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).SetRawJsonValueAsync(json);
            reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(assignment.assignmentName).SetRawJsonValueAsync(json);
        }
        /*Assignment assignment = new Assignment(assignmentInput.text, 1, 2, 3, "1",
                "1", 100, "1", "1", "1", "1");*/
        //SetValueAsync(int/str) accept interger or string data type
        


    }

    public List<Assignment> LoadAssignment()
    {
        Debug.LogFormat("----HERE---");
        id = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----USER INFO ID---" + id);

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
                    if (assignmentNode.Key == id)
                    {
                        //load user data into player object
                        var assignmentDict = (IDictionary<string, object>)assignmentNode.Value;
                        foreach (KeyValuePair<string, object> kvp1 in assignmentDict)
                        {
                            Debug.LogFormat("ASSIGNMENT ---- Key = {0}, Value = {1}", kvp1.Key, kvp1.Value);
                        }
                        assignmentData = new Assignment(assignmentDict);
                        assignmentList.Add(assignmentData);
                    }
                }
                Debug.Log("Assignment Name: " + assignmentData.assignmentName);
                
            }

        });
        return assignmentList;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
