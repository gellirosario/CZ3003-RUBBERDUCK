using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class QuestionController : MonoBehaviour
{
    private DatabaseReference reference;
    public Button createQuestionBtn;
    public Button deleteQuestionBtn;

    public InputField answerInputField;
    public InputField questionInputField;
    public InputField option1InputField;
    public InputField option2InputField;
    public InputField option3InputField;
    public InputField option4InputField;
    public Dropdown dropdownWorld;
    public Dropdown dropdownTopic;
    public Dropdown dropdownLevel;
    public InputField levelInputField;

    public List<Question> questionList_All = new List<Question>();


    // Use this for initialization
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

        //print("Before loading questionlist count: " + questionList.Count);
        //if (questionList.Count == 0)
        //{
          //  questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1,1);
        //}
    }

   

    public void SaveQuestion()
    {

        int qid=0;

        Debug.LogFormat("Question: " + questionInputField.text);

        print("Question: " + questionInputField.text);
        print("Answer: " + answerInputField.text);

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");

        // Get the root reference location of the database.
       reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.GetValueAsync().ContinueWith(task =>
        {
        if (task.IsFaulted)
        {
            // Handle the error...
            Debug.Log("Error in data retrieval");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Received values for Questions.");

            DataSnapshot snapshot = task.Result;

            //print("test: " + snapshot.Child("Questions").ChildrenCount);
            qid = (int)snapshot.Child("Questions").ChildrenCount + 1;
            print("QID" + qid);

                //LocPickerString = LocationPicker.GetComponent.< UI.Dropdown > ().itemText.text
                //print("Level" + dropdownLevel.options[dropdownLevel.value].text);
                //string diff = dropdownLevel.options[dropdownLevel.value].text.ToString();
                //add
                //print(diff);

                Question question = new Question(qid, dropdownWorld.value, dropdownTopic.value, levelInputField.text
                 , questionInputField.text, int.Parse(answerInputField.text),
                option1InputField.text, option2InputField.text, option3InputField.text, option4InputField.text);
                string json = JsonUtility.ToJson(question);
                
                print("test:" + json);
                reference.Child("Questions").Child(qid.ToString()).SetRawJsonValueAsync(json);
            }
        });

       

        /*Question question = new Question(1234, 123, 274,
                    "difficulty","question", 456,
                    "option1", "option2", "option3","option4");
                string json = JsonUtility.ToJson(question);
                reference.Child("Questions").Child("1234").SetRawJsonValueAsync(json);*/
        

       // Question question = new Question("questionid",dropdownWorld.value, dropdownTopic.value,
       //             dropdownLevel.value.ToString() , questionInputField.text, int.Parse(answerInputField.text),
       //             option1InputField.text , option2InputField.text, option3InputField.text, option4InputField.text);
       // string json = JsonUtility.ToJson(question);
       // reference.Child("Questions").Child("UserID").SetRawJsonValueAsync(json);

        //print("After loading questionlist count: " + questionList.Count);
        //for (int i = 0; i < questionList.Count; i++)
        //{

        //Assignment assignment = new Assignment(createAssignmentInput.text, questionList[i].qnID, questionList[i].world, questionList[i].stage, questionList[i].difficulty,
        //  questionList[i].question, questionList[i].answer, questionList[i].option1, questionList[i].option2, questionList[i].option3, questionList[i].option4);
        //string json = JsonUtility.ToJson(assignment);
        //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).SetRawJsonValueAsync(json);
        //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(assignment.assignmentName).SetRawJsonValueAsync(json);


        //}
    }

    public void DeleteQuestion()
    {
        //Debug.LogFormat("Question: " + questionInputField.text);


        //print(reference);
        //print(reference.Child("Questions").Child("1234"));

        //reference.Child("Questions").Child("1234").RemoveValueAsync();
        //displayDeleteMessage.text = deleteAssignmentInput.text + " Successfully Deleted" ;

    }
}