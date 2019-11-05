using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

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
    public Dropdown dropdownAnswer;
    public Question questionData { get; private set; }


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

        int qid = 0;

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

                Question question = new Question(qid, dropdownWorld.value+1, dropdownTopic.value+1, (dropdownLevel.value+1).ToString()
                 , questionInputField.text, dropdownAnswer.value+1,
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

    public void OnTopicDropdownChange()
    {
        int topic = dropdownWorld.value;

        switch (topic)
        {
            case 0:
                dropdownTopic.options[0].text = "1. Software Engineering Principles";
                dropdownTopic.options[1].text = "2. Requirements Analysis";
                dropdownTopic.options[2].text = "3. Modelling";
                dropdownTopic.RefreshShownValue();
                break;
            case 1:
                dropdownTopic.options[0].text = "1. Architectural Designs";
                dropdownTopic.options[1].text = "2. Design Concepts";
                dropdownTopic.options[2].text = "3. Component Level Designs";
                dropdownTopic.RefreshShownValue();
                break;
            case 2:
                dropdownTopic.options[0].text = "1. Software Elements";
                dropdownTopic.options[1].text = "2. Software Components";
                dropdownTopic.options[2].text = "3. Software Configuration";
                dropdownTopic.RefreshShownValue();
                break;
            case 3:
                dropdownTopic.options[0].text = "1. Software Testing Techniques and Strategies";
                dropdownTopic.options[1].text = "2. Testing Application";
                dropdownTopic.options[2].text = "3. Software Testing";
                dropdownTopic.RefreshShownValue();
                break;
            case 4:
                dropdownTopic.options[0].text = "1. Software Management";
                dropdownTopic.options[1].text = "2. Software Configuration";
                dropdownTopic.options[2].text = "3. Quality Management";
                dropdownTopic.RefreshShownValue();
                break;
            default:
                break;
        }
    }


    public void LoadQuestion()
    {
        Debug.LogFormat("----HERE---");
        Debug.LogFormat("----QUESTION INFO---");

        FirebaseDatabase.DefaultInstance.GetReference("Questions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Question table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot qnid in snapshot.Children)
                {
                    var qnIDDict = (IDictionary<string, object>)qnid.Value;

                    foreach (KeyValuePair<string, object> kvp in qnIDDict)
                    {
                        Debug.LogFormat("QUESTION DETAILS ---- Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                    }


                    questionData = new Question(qnIDDict);
                    questionList_All.Add(questionData);
                }
            }
            

            print("Current list of questions in question id: " + questionList_All.Count);
            PlayerPrefs.SetInt("QuestionListCount", questionList_All.Count);

            for (var i = 0; i < questionList_All.Count; i++)
            {
                PlayerPrefs.SetInt("QuestionList(qnID)" + i, questionList_All[i].qnID);
                PlayerPrefs.SetInt("QuestionList(world)" + i, questionList_All[i].world);
                PlayerPrefs.SetInt("QuestionList(stage)" + i, questionList_All[i].stage);
                PlayerPrefs.SetString("QuestionList(difficulty)" + i, questionList_All[i].difficulty);
                PlayerPrefs.SetString("QuestionList(question)" + i, questionList_All[i].question);
                PlayerPrefs.SetInt("QuestionList(answer)" + i, questionList_All[i].answer);
                PlayerPrefs.SetString("QuestionList(option1)" + i, questionList_All[i].option1);
                PlayerPrefs.SetString("QuestionList(option2)" + i, questionList_All[i].option2);
                PlayerPrefs.SetString("QuestionList(option3)" + i, questionList_All[i].option3);
                PlayerPrefs.SetString("QuestionList(option4)" + i, questionList_All[i].option4);
            }
        });
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