﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class CreateChallengeController : MonoBehaviour
{
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

    public Challenge newChallenge;

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

    //toggles the popup window on challenge creation
    public void TogglePopup()
    {
        if(popup != null)
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


    public void CreateChallenge()
    {
        string difficultySelected = difficultyDropdown.options[difficultyDropdown.value].text;
        world = worldDropdown.value + 1;
        stage = stageDropdown.value + 1;

        List<int> questionList = RandomizeQuestions(int.Parse(noOfQns.text), world, stage, difficultySelected);

        //get name of challenge creator
        string creator = ProfileLoader.userData.name;

        newChallenge = new Challenge(questionList, creator);

        Debug.Log("Challenge created with ID of " + newChallenge.challengeId);

        Debug.Log("== Challenge Questions ==");
        
        foreach(int id in newChallenge.challengeQns)
        {
            Debug.Log("Question ID: " + id);
        }

        //Add to real time database
        string json = JsonUtility.ToJson(newChallenge);
        reference.Child("Challenges").Child(newChallenge.challengeId).SetRawJsonValueAsync(json);

        idText.text = "Challenge ID: " + newChallenge.challengeId;
        PlayerPrefs.SetString("NewChallengeID", newChallenge.challengeId);

        TogglePopup();
    }
    //generates a randomized list of questions with specified difficulty, world, stage, and number of questions
    private List<int> RandomizeQuestions(int NoOfQns, int world, int stage, string difficulty)
    {
        List<int> questionList = new List<int>();

        //get filtered list from QuestionLoader
        List<Question> worldStageList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(world, stage);
        Debug.Log("Filtered list by world " + world + ", stage " + stage);

        //foreach (Question q in worldStageList)
        //{
        //    Debug.Log("Question ID: " + q.qnID + "  world: " + q.stage + "  stage: " + q.world + "  difficulty: " + q.difficulty);
        //}

        //filter further by difficulty
        List<Question> filteredList = QuestionLoader.Instance.FilterQuestionsListByDifficulty(worldStageList, difficulty);
        Debug.Log("Filtered list by difficulty: " + difficulty);
        Debug.Log("Filtered list has " + filteredList.Count + " items.");

        //foreach(Question q in filteredList)
        //{
        //    Debug.Log("Question ID: " + q.qnID + "  world: " + q.stage + "  stage: " + q.world + "  difficulty: " + q.difficulty);
        //}

        //shuffle list
        questionList = ShuffleList(filteredList, NoOfQns);

        return questionList;
    }


    //function to shuffle question list
    private List<int> ShuffleList(List<Question> questionList, int NoOfQns)
    {
        List<int> randomList = new List<int>();

        if (questionList.Count < NoOfQns)
        {
            warningText.text = "Warning: Not enough questions in database, so only " + questionList.Count + " questions are included in this challenge.";
        }

        int randomIndex = 0;

        while(questionList.Count > 0 && NoOfQns > 0)
        {
            randomIndex = Random.Range(0, questionList.Count);
            randomList.Add(questionList[randomIndex].qnID);
            //Debug.Log("Added index " + randomIndex + " to new list");
            //Debug.Log("New list now has " + randomList.Count + " items");
            //remove from original list to avoid duplicates
            questionList.RemoveAt(randomIndex);
            NoOfQns--;
        }

        return randomList;
    }

    public void PlayChallenge()
    {
        PlayerPrefs.SetString("challengeID", newChallenge.challengeId);
        QuestionLoader.Instance.challenge = newChallenge;
        SceneManager.LoadScene("ChallengeLevel");
    }
}
