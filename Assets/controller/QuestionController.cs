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
    public string uid;
    public Text displayDeleteMessage;
    public InputField deleteQuestionInput;
    public InputField answerInputField;
    public InputField questionInputField;
    public InputField option1InputField;
    public InputField option2InputField;
    public InputField option3InputField;
    public InputField option4InputField;
    public Dropdown dropdownWorld;
    public Dropdown dropdownStage;
    public Dropdown dropdownDifficulty;
    public Question questionData { get; private set; }


    public List<Question> questionList_All = new List<Question>();


    // Use this for initialization
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
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
                print("TOTAL CHILD VALUE" + snapshot.Child("Questions").ChildrenCount);


                //LocPickerString = LocationPicker.GetComponent.< UI.Dropdown > ().itemText.text
                //print("Level" + dropdownLevel.options[dropdownLevel.value].text);
                //string diff = dropdownLevel.options[dropdownLevel.value].text.ToString();
                //add
                //print(diff);

                Question question = new Question(qid, dropdownWorld.value, dropdownStage.value, dropdownDifficulty.value.ToString()
                 , questionInputField.text, int.Parse(answerInputField.text),
                option1InputField.text, option2InputField.text, option3InputField.text, option4InputField.text);
                string json = JsonUtility.ToJson(question);

                print("test:" + json);
                reference.Child("Questions").Child(qid.ToString()).SetRawJsonValueAsync(json);
            }
        });
    }

    public void OnTopicDropdownChange()
    {
        int topic = dropdownWorld.value;

        switch (topic)
        {
            case 0:
                dropdownStage.options[0].text = "1. Software Engineering Principles";
                dropdownStage.options[1].text = "2. Requirements Analysis";
                dropdownStage.options[2].text = "3. Modelling";
                dropdownStage.RefreshShownValue();
                break;
            case 1:
                dropdownStage.options[0].text = "1. Architectural Designs";
                dropdownStage.options[1].text = "2. Design Concepts";
                dropdownStage.options[2].text = "3. Component Level Designs";
                dropdownStage.RefreshShownValue();
                break;
            case 2:
                dropdownStage.options[0].text = "1. Software Elements";
                dropdownStage.options[1].text = "2. Software Components";
                dropdownStage.options[2].text = "3. Software Configuration";
                dropdownStage.RefreshShownValue();
                break;
            case 3:
                dropdownStage.options[0].text = "1. Software Testing Techniques and Strategies";
                dropdownStage.options[1].text = "2. Testing Application";
                dropdownStage.options[2].text = "3. Software Testing";
                dropdownStage.RefreshShownValue();
                break;
            case 4:
                dropdownStage.options[0].text = "1. Software Management";
                dropdownStage.options[1].text = "2. Software Configuration";
                dropdownStage.options[2].text = "3. Quality Management";
                dropdownStage.RefreshShownValue();
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


            /*//get filtered list from QuestionLoader
            List<Question> worldStageList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(dropdownWorld.value, dropdownStage.value);
            Debug.Log("Filtered list by world " + dropdownWorld.value + ", stage " + dropdownStage.value);

            //foreach (Question q in worldStageList)
            //{
            //    Debug.Log("Question ID: " + q.qnID + "  world: " + q.stage + "  stage: " + q.world + "  difficulty: " + q.difficulty);
            //}

            //filter further by difficulty
            List<Question> filteredList = QuestionLoader.Instance.FilterQuestionsListByDifficulty(worldStageList, dropdownDifficulty.value.ToString());
            Debug.Log("Filtered list by difficulty: " + dropdownDifficulty.value.ToString());
            Debug.Log("Filtered list has " + filteredList.Count + " items.");

            print("==============================================================");
            print("Current filitered list of questions : " + filteredList.Count);
            print("==============================================================");
            PlayerPrefs.SetInt("QuestionListCount", filteredList.Count);*/




            print("Current list of questions in question id: " + questionList_All.Count);
            PlayerPrefs.SetInt("QuestionListCount", questionList_All.Count);

            for (var i = 0; i < questionList_All.Count; i++)
            {
                if (((dropdownWorld.value + 1) == questionList_All[i].world) && ((dropdownStage.value + 1) == questionList_All[i].stage) && (dropdownDifficulty.value.ToString() == questionList_All[i].difficulty))
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
            }
        });
    }

    public void DeleteQuestion()
    {
        bool questionIdFound = false;
        Debug.LogFormat("----HERE---");
        uid = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----Question INFO ID---" + uid);

        FirebaseDatabase.DefaultInstance.GetReference("Questions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Question table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                print("=================================REACH DELETE QUESTION 1===================================");
                foreach (DataSnapshot questionNode in snapshot.Children)
                {
                    print("=================================REACH DELETE QUESTION 2===================================");

                    //var questiontDict = (IDictionary<string, object>)questionNode.Value;

                    // Debug.Log("QUESTION NAME:" + key);
                    Debug.Log("Inputted Text:" + deleteQuestionInput.text);
                    print("QUESTION KEY" + questionNode.Key);

                    if (questionNode.Key == deleteQuestionInput.text.ToString())
                    {
                        reference.Child("Questions").Child(questionNode.Key).RemoveValueAsync();

                        displayDeleteMessage.text = deleteQuestionInput.text + "\n Successfully Deleted";
                        questionIdFound = true;
                        break;
                    }
                }
            }
        });

        if (!questionIdFound)
        {
            displayDeleteMessage.text = deleteQuestionInput.text + "\n Is Not Found In The Database";
        }

        Invoke("ClearMessage", 3);
    }

    void ClearMessage()
    {
        deleteQuestionInput.text = "";
        displayDeleteMessage.text = "";
    }

}