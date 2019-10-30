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
    private List<Question> questionList = new List<Question>();
    // Start is called before the first frame update
    void Start()
    {
        // Retrieve Question List According to World and Stage
        print("Before loading questionlist count: " + questionList.Count);
        if (questionList.Count == 0)
        {
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1,1);
            print("After loading questionlist count: " + questionList.Count);
        }
    }

    public void SaveAssignmentToDatabase()
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
            reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).SetRawJsonValueAsync(json);
        }
        /*Assignment assignment = new Assignment(assignmentInput.text, 1, 2, 3, "1",
                "1", 100, "1", "1", "1", "1");*/
        //SetValueAsync(int/str) accept interger or string data type
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
