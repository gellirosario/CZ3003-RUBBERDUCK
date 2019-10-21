using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LevelController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;
    
    public Text questionTxt, levelTxt, o1Text, o2Text, o3Text, o4Text;
    public Button option1Btn, option2Btn, option3Btn, option4Btn;
    
    private List<Question> questionList = new List<Question>();
    private List<Question> questionList_Filtered = new List<Question>();
    private bool doUpdate = false;
    private bool isFirst = false;
    private bool isCorrect = false;
    
    private string difficulty;
    private int level;
    private int score;
    private int randomQuestionNo;
    
    private Color colorGreen = new Color(0,198,0);
    private Color colorRed = new Color(255,0,0);
    
    public void Awake()
    {
        levelTxt.text = "Level " + "";
        questionTxt.text = "";
        o1Text.text = "";
        o2Text.text = "";
        o3Text.text = "";
        o4Text.text = "";
        
        option1Btn = GetComponent<Button>();
        option2Btn = GetComponent<Button>();
        option3Btn = GetComponent<Button>();
        option4Btn = GetComponent<Button>();
        
        difficulty = "Medium"; // First level = Medium
        isFirst = true;
        level = 0;
        score = 0;

        if (questionList.Count == 0)
        {
            // Retrieve Question List According to World and Stage
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(PlayerPrefs.GetInt("SelectedWorld"),
                PlayerPrefs.GetInt("SelectedStage"));
        }
        
        Debug.Log(questionList);
        
        doUpdate = true;
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }
    
    private void Update()
    {
        if (doUpdate)
            UpdateUI();
    }

    private void UpdateUI()
    {
        // Check if stage hasn't ended (lvl 10 is the last stage)
        if (level < 10)
        {
            
            if (doUpdate == true && questionList != null)
            {
                level = level + 1; // Update level
                levelTxt.text = "LEVEL " + level.ToString();

                // Set question and options
                if (!isFirst)
                {
                    if (isCorrect)
                    {
                        switch (difficulty)
                        {
                            case "Easy":
                                difficulty = "Medium";
                                break;
                            case "Medium":
                                difficulty = "Hard";
                                break;
                        }
                        
                    }
                    else
                    {
                        switch (difficulty)
                        {
                            case "Medium":
                                difficulty = "Easy";
                                break;
                            case "Hard":
                                difficulty = "Medium";
                                break;
                        }
                    }
                }
                
                // Filter question list according to difficulty
                questionList_Filtered = QuestionLoader.Instance.FilterQuestionsListByDifficulty(questionList, difficulty); 
                randomQuestionNo = Random.Range(0, questionList_Filtered.Count);
                Debug.LogFormat("Level : " + level.ToString());
                Debug.LogFormat("Difficulty : " + difficulty);
                Debug.LogFormat("Question List Filtered No : " + questionList_Filtered.Count.ToString());
                Debug.LogFormat("Random Number : " + randomQuestionNo.ToString());
                
                questionTxt.text = questionList_Filtered[randomQuestionNo].question;
                o1Text.text = "a. " + questionList_Filtered[randomQuestionNo].option1;
                o2Text.text = "b. " + questionList_Filtered[randomQuestionNo].option2;
                o3Text.text = "c. " + questionList_Filtered[randomQuestionNo].option3;
                o4Text.text = "d. " + questionList_Filtered[randomQuestionNo].option4;
                
                isFirst = false; // Not first question
                doUpdate = false; // Set to false until the player answered
            }

        }
        else
        {
            doUpdate = false;
            EndStage();
        }
        
    }

    // Check selection option
    public void CheckAnswer(int selectedOption)
    {
        Debug.Log("Correct Answer = " + questionList_Filtered[randomQuestionNo].answer.ToString());
        Debug.Log("Selected Answer = " + questionList_Filtered[randomQuestionNo].answer.ToString());

        if (selectedOption == questionList_Filtered[randomQuestionNo].answer)
        {
            
            int scoreGiven = 0;

            switch (difficulty)
            {
                case "Easy":
                    scoreGiven = 10;
                    break;
                case "Medium":
                    scoreGiven = 20;
                    break;
                case "Hard":
                    scoreGiven = 30;
                    break;
            }
            
            score = score + scoreGiven;
            
            
            if (level > 1) // After first question
            {
                if (isCorrect)
                {
                    questionTxt.text = "Correct!";
                }
            }
            
            doUpdate = false;
        }
        else
        {
           
        }
        
        Debug.Log("Score = " + score.ToString());
        
        doUpdate = true;
        
        for (int i = 0; i < questionList.Count; i++)
        {
            if (questionList[i].qnID == questionList_Filtered[randomQuestionNo].qnID)
            {
                questionList.RemoveAt(i);
            }
        }
    }

    // Check whether pass or fail
    public void EndStage()
    {
        
    }
}
    
    

