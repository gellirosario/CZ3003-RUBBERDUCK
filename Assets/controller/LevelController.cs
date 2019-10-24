using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public HealthBar healthbarPlayer, healthbarEnemy;
    private HealthSystem healthSystemPlayer;
    private HealthSystem healthSystemEnemy;

    private List<Question> questionList = new List<Question>();
    private List<Question> questionList_Filtered = new List<Question>();
    private bool doUpdate = false;
    private bool isFirst = false;
    private bool isCorrect = false;
    private bool isChecked = false;
    
    private string difficulty;
    private int level;
    private int score;
    private int randomQuestionNo;
    private int qnWrong;
    
    private Color colorGreen = new Color(0,198,0);
    private Color colorRed = new Color(255,0,0);

    public Animator character1Anim;
    public Animator enemy1Anim;
    
    private Player currentPlayer;
    
    public void Start()
    {
        healthSystemPlayer = new HealthSystem(30);
        healthbarPlayer.Setup(healthSystemPlayer);
        healthSystemEnemy = new HealthSystem(50);
        healthbarEnemy.Setup(healthSystemEnemy);
    }
    
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
        qnWrong = 0;

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
        {
            UpdateUI();
        }

    }

    private void UpdateUI()
    {
        // Check if stage hasn't ended (lvl 10 is the last stage)
        if (healthSystemEnemy.GetHealth() != 0 || healthSystemPlayer.GetHealth() != 0)
        {
            if (doUpdate == true && questionList != null)
            {
                
                if (healthSystemPlayer.GetHealth() == 10)
                {
                    character1Anim.SetBool("isSick",true);
                }
                else
                {
                    character1Anim.SetBool("isReady",true);
                    character1Anim.SetTrigger("Ready");
                }
                enemy1Anim.SetTrigger("Idle");

                level = level + 1; // Update level
                levelTxt.text = "LEVEL " + level.ToString();

                // Set question and options
                if (!isFirst)
                {
                    // Set Difficulty according if the previous answer is correct
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
            
            questionTxt.text = "Game Over!"; 
            o1Text.text = "";
            o2Text.text = "";
            o3Text.text = "";
            o4Text.text = "";
        }
        
    }

    // Check selection option
    public void CheckAnswer(int selectedOption)
    {
        Debug.Log("Correct Answer = " + questionList_Filtered[randomQuestionNo].answer.ToString());
        Debug.Log("Selected Answer = " + questionList_Filtered[randomQuestionNo].answer.ToString());

        if (selectedOption == questionList_Filtered[randomQuestionNo].answer)
        {
         
            character1Anim.SetTrigger("Stabbing");
            enemy1Anim.SetTrigger("Damage");
            
            isCorrect = true;
            
            int scoreGiven = 0;

            questionTxt.text = "Correct!";
            
            healthSystemEnemy.Damage(10);
            
            // Enemy HP is 0
            if (healthSystemEnemy.GetHealth() == 0)
            {
                character1Anim.SetTrigger("Victory");
                enemy1Anim.SetTrigger("Down");
                
                
                // End Stage
                Invoke("EndStage", 2);
            }
            else if (healthSystemPlayer.GetHealth() == 10)
            {
                character1Anim.SetBool("isSick",true);
            }
            else
            {
                character1Anim.SetTrigger("Ready");
            }
            
            enemy1Anim.SetTrigger("Idle");
                
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
        }
        else
        {
            // Set no. of QN Wrong
            qnWrong += 1;
            
            character1Anim.SetTrigger("Damage");
            enemy1Anim.SetTrigger("Swinging");
            
            isCorrect = false;
            
            questionTxt.text = "Wrong!";

            healthSystemPlayer.Damage(10);
            
            // Character HP is 0
            if (healthSystemPlayer.GetHealth() == 0)
            {
                character1Anim.SetTrigger("Down");
                enemy1Anim.SetTrigger("Idle");
                
                // Set Score to 0 (Stage Fail)
                score = 0;
                
                // End Stage
                Invoke("EndStage", 2);
            }
            else if (healthSystemPlayer.GetHealth() == 10)
            {
                character1Anim.SetBool("isSick",true);
            }
            else
            {
                character1Anim.SetBool("isReady",true);
            }
            
            enemy1Anim.SetTrigger("Idle");
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
        if (score != 0)
        {
            PlayerPrefs.SetInt("Score", score);
            SavePlayerScore();
        }
        else
        {
            PlayerPrefs.SetInt("Score", 0);
        }
        
        Debug.Log("Preferences set: Score - " + score.ToString());
        if (PlayerPrefs.GetInt("Score") != null)
        {
            SceneManager.LoadScene("StageClear");
        }
        else
        {
            SceneManager.LoadScene("StageFail");
        }
    }

    public void SavePlayerScore()
    {
        currentPlayer = ProfileLoader.Instance.playerData;

        level = level - 1;
        
        int totalScore = currentPlayer.totalPoints + score;
        int totalQnAnswered = currentPlayer.totalQnAnswered + level;
        
        reference.Child("Player").Child(PlayerPrefs.GetString("UserID")).Child("totalPoints").SetValueAsync(totalScore);
        reference.Child("Player").Child(PlayerPrefs.GetString("UserID")).Child("totalQnAnswered").SetValueAsync(totalQnAnswered);

        string worldStage = "world" + PlayerPrefs.GetInt("SelectedWorld").ToString() + "stage" +
                            PlayerPrefs.GetInt("SelectedStage");
        
        int stars = 0;
        double percentageWrong = 0;

        percentageWrong = qnWrong / level;
        if (percentageWrong == 0)
        {
            stars = 3;
        }
        else if (percentageWrong >= 0.5)
        {
            stars = 2;
        }
        
        reference.Child("Player").Child(PlayerPrefs.GetString("UserID")).Child("mastery").Child(worldStage).SetValueAsync(stars);
    }
}
    
    

