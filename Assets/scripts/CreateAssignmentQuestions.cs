using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class CreateAssignmentQuestions : MonoBehaviour
{
    public InputField createAssignmentInput;
    public Text noOfQns;
    public Dropdown difficultyDropdown, worldDropdown, stageDropdown;

    public GameObject popup;
    public Text idText, warningText;

    public const int MIN_QNS = 1;
    public const int MAX_QNS = 10;

    public string difficulty;
    public int world;
    public int stage;

    private FirebaseApp app;
    private DatabaseReference reference;

    private void Start()
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

    public void OnTopicDropdownChange()
    {
        int topic = worldDropdown.value;

        switch (topic)
        {
            case 0:
                stageDropdown.options[0].text = "1. Software Engineering Principles";
                stageDropdown.options[1].text = "2. Requirements Analysis";
                stageDropdown.options[2].text = "3. Modelling";
                stageDropdown.RefreshShownValue();
                break;
            case 1:
                stageDropdown.options[0].text = "1. Architectural Designs";
                stageDropdown.options[1].text = "2. Design Concepts";
                stageDropdown.options[2].text = "3. Component Level Designs";
                stageDropdown.RefreshShownValue();
                break;
            case 2:
                stageDropdown.options[0].text = "1. Software Elements";
                stageDropdown.options[1].text = "2. Software Components";
                stageDropdown.options[2].text = "3. Software Configuration";
                stageDropdown.RefreshShownValue();
                break;
            case 3:
                stageDropdown.options[0].text = "1. Software Testing Techniques and Strategies";
                stageDropdown.options[1].text = "2. Testing Application";
                stageDropdown.options[2].text = "3. Software Testing";
                stageDropdown.RefreshShownValue();
                break;
            case 4:
                stageDropdown.options[0].text = "1. Software Management";
                stageDropdown.options[1].text = "2. Software Configuration";
                stageDropdown.options[2].text = "3. Quality Management";
                stageDropdown.RefreshShownValue();
                break;
            default:
                break;
        }
    }

    //toggles the popup window on assignment creation
    public void TogglePopup()
    {
        if (popup != null)
        {
            bool isActive = popup.activeSelf;
            popup.SetActive(!isActive);
        }
    }

    public void IncrementNoOfQns()
    {
        int val = int.Parse(noOfQns.text);
        if (val < MAX_QNS)
            val++;
        noOfQns.text = val.ToString();
    }

    public void DecrementNoOfQns()
    {
        int val = int.Parse(noOfQns.text);
        if (val > MIN_QNS)
            val--;
        noOfQns.text = val.ToString();
    }


    public void CreateQuestions()
    {
        string difficultySelected = difficultyDropdown.options[difficultyDropdown.value].text;
        world = worldDropdown.value + 1;
        stage = stageDropdown.value + 1;

        //some test code for now
        List<int> questionList = GenerateQuestions(int.Parse(noOfQns.text), world, stage, difficultySelected);

        //get name of assignment creator
        //string creator = ProfileLoader.Instance.userData.name;
        string creatorUID = PlayerPrefs.GetString("UserID");

        Assignment newAssignment = new Assignment(createAssignmentInput.text, questionList, creatorUID);

        Debug.Log("Assignment created with ID of " + newAssignment.assignmentId);

        Debug.Log("== Assignment Questions ==");

        foreach (int id in newAssignment.assignmentQns)
        {
            Debug.Log("Question ID: " + id);
        }

        //Add to real time database

        string json = JsonUtility.ToJson(newAssignment);
        reference.Child("Assignment2").Child(newAssignment.assignmentId).SetRawJsonValueAsync(json);

        idText.text = "Assignment ID: " + newAssignment.assignmentId;
        PlayerPrefs.SetString("NewAssignmentID", newAssignment.assignmentId);

        TogglePopup();
    }

    //generates a randomized list of questions with specified difficulty, world, stage, and number of questions
    private List<int> GenerateQuestions(int NoOfQns, int world, int stage, string difficulty)
    {
        List<int> questionList = new List<int>();

        //get filtered list from QuestionLoader
        List<Question> worldStageList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(world, stage);
        Debug.Log("Filtered list by world " + world + ", stage " + stage);

        for (int i = 0; i < worldStageList.Count; i++)
        {
            if (worldStageList[i].qnID == i)
            {
                Debug.Log("worldandstage Question ID: " + worldStageList[i].qnID);
            }
            
        }
        //foreach (Question q in worldStageList)
        //{
        //    Debug.Log("Question ID: " + q.qnID + "  world: " + q.stage + "  stage: " + q.world + "  difficulty: " + q.difficulty);
        //}

        //filter further by difficulty
        List<Question> filteredList = QuestionLoader.Instance.FilterQuestionsListByDifficulty(worldStageList, difficulty);
        Debug.Log("Filtered list by difficulty: " + difficulty);
        Debug.Log("Filtered list has " + filteredList.Count + " items.");

        for (int i = 0; i < filteredList.Count; i++)
        {
            Debug.Log("difficulty Question ID: " + filteredList[i].qnID);
        }
        //foreach(Question q in filteredList)
        //{
        //    Debug.Log("Question ID: " + q.qnID + "  world: " + q.stage + "  stage: " + q.world + "  difficulty: " + q.difficulty);
        //}

        //shuffle list
        questionList = SelectQuestionsList(filteredList, NoOfQns);

        return questionList;
    }


    //function to shuffle question list
    private List<int> SelectQuestionsList(List<Question> questionList, int NoOfQns)
    {
        List<int> qnList = new List<int>();
        
        if (questionList.Count < NoOfQns)
        {
            warningText.text = "Warning: Not enough questions in database, so only " + questionList.Count + " questions are included in this assignment.";
        }

        int questionNumber = 1;
        int i = 0;
        //NoOfQns += 1;

        while (questionList.Count > 0 && NoOfQns > 0)
        {

            //Debug.Log("BEFORE INDEX: " + questionNumber);
            //Debug.Log("BEFORE ID " + questionList[i].qnID);
            if (questionList[i].qnID == questionNumber)
            {
                
                qnList.Add(questionList[i].qnID);
                questionNumber++;
                NoOfQns--;
                //Debug.Log("No of question left to add: " + NoOfQns);

                //Debug.Log("ADDED INDEX: " + questionNumber);
                //Debug.Log("ADDED ID " + questionList[i].qnID);

                if (i == 9)
                {
                    i = 9;
                    continue;
                }
            }
            i++;
            //print("==========INDEX OF I========= " + i);
            //Debug.Log("Added index " + Index + " to new list");
            //Debug.Log("New list now has " + qnList.Count + " items");
            //remove from original list to avoid duplicates
            //index++;

            //if (i == 10)
            //    i--;


            if (i > questionList.Count)
                i = 0;


        }

        return qnList;
    }

}
