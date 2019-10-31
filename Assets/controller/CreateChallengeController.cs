using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class CreateChallengeController : MonoBehaviour
{
    public Text noOfQns;
    public Dropdown difficultyDropdown, worldDropdown, stageDropdown;

    public GameObject popup;
    public Text idText;

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

        //some test code for now
        List<int> questionList = RandomizeQuestions(int.Parse(noOfQns.text), world, stage, difficultySelected);

        Challenge newChallenge = new Challenge(questionList);

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
            Debug.Log("Not enough questions in database, so only " + questionList.Count + " questions will be included.");
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
}
