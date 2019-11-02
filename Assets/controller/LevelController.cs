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

    public Text questionTxt, levelTxt, o1Text, o2Text, o3Text, o4Text;
    public Button option1Btn, option2Btn, option3Btn, option4Btn;

    public Text scoreTxt;
    public Text difficultyTxt;


    public HealthBar healthbarPlayer, healthbarEnemy;
    private HealthSystem healthSystemPlayer;
    private HealthSystem healthSystemEnemy;

    private List<Question> questionList = new List<Question>();
    private List<Question> questionList_Filtered_Easy = new List<Question>();
    private List<Question> questionList_Filtered_Normal = new List<Question>();
    private List<Question> questionList_Filtered_Hard = new List<Question>();

    private string qn, o1, o2, o3, o4; // For the UI
    private int answer; // For the UI

    private bool doUpdate = false;
    private bool isFirst = false;
    private bool isCorrect = false;
    private bool isChecked = false;

    private string difficulty;
    private int level;
    private int score;
    private int randomQuestionNo;
    private int qnWrong;
    private int rightansNo = 0;

    private string worldAndStage;  //ss ==
    private int appear = 0;//ss ==
    private int correctAns = 0;//ss ==
    private int wrongAns = 0;//ss ==



    private Color colorGreen = new Color(0, 198, 0);
    private Color colorRed = new Color(255, 0, 0);

    public Animator character1Anim;
    public Animator character2Anim;
    public Animator enemy1Anim;
    public Animator enemy2Anim;
    public Animator enemy3Anim;

    private Player currentPlayer;

    public GameObject charSprite1;
    public GameObject charSprite2;
    public GameObject enemySprite1;
    public GameObject enemySprite2;
    public GameObject enemySprite3;

    private int selectedChar;
    private int randomEnemy;

   // private Report currentReport;


    public void Start()
    {
        healthSystemPlayer = new HealthSystem(30);
        healthbarPlayer.Setup(healthSystemPlayer);
        healthSystemEnemy = new HealthSystem(50);
        healthbarEnemy.Setup(healthSystemEnemy);

        difficultyTxt.text = "Difficulty: ";
        scoreTxt.text = "Score: 0";
    }

    public void Awake()
    {
        selectedChar = PlayerPrefs.GetInt("CharacterID");
        randomEnemy = Random.Range(0, 2);

        switch(selectedChar){
            case 0:
                charSprite2.GetComponent<Renderer>().enabled = false;
                break;
            case 1:
                charSprite1.GetComponent<Renderer>().enabled = false;
                break;
        }

        switch(randomEnemy){
            case 0:
                enemySprite2.GetComponent<Renderer>().enabled = false;
                enemySprite3.GetComponent<Renderer>().enabled = false;
                break;
            case 1:
                enemySprite1.GetComponent<Renderer>().enabled = false;
                enemySprite3.GetComponent<Renderer>().enabled = false;
                break;
            case 2:
                enemySprite1.GetComponent<Renderer>().enabled = false;
                enemySprite2.GetComponent<Renderer>().enabled = false;
                break;
        }

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

        difficulty = "Normal"; // First level = Normal
        
        isFirst = true;
        level = 0;
        score = 0;
        qnWrong = 0;

        if (questionList.Count == 0)
        {
            // Retrieve Question List According to World and Stage
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(PlayerPrefs.GetInt("SelectedWorld"),
                PlayerPrefs.GetInt("SelectedStage"));

            // for report use ============= ss
            worldAndStage = "w" + PlayerPrefs.GetInt("SelectedWorld").ToString() +
                            "s" + PlayerPrefs.GetInt("SelectedStage").ToString();
            //==========================

            // Retrieve Question List According to Difficulty
            questionList_Filtered_Easy = QuestionLoader.Instance.FilterQuestionsListByDifficulty(questionList, "Easy");
            questionList_Filtered_Normal = QuestionLoader.Instance.FilterQuestionsListByDifficulty(questionList, "Normal");
            questionList_Filtered_Hard = QuestionLoader.Instance.FilterQuestionsListByDifficulty(questionList, "Hard");
        }

        doUpdate = true;

        difficultyTxt.text = "Difficulty: Normal";

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

    private void Update()
    {
        if (doUpdate)
        {
            UpdateUI();
        }

    }

    private void UpdateUI()
    {
        // Check if stage hasn't ended (lvl 7 is the last stage)
        if (healthSystemEnemy.GetHealth() != 0 || healthSystemPlayer.GetHealth() != 0)
        {
            if (doUpdate == true && questionList != null)
            {

                if (healthSystemPlayer.GetHealth() == 10)
                {
                    switch(selectedChar){
                        case 0:
                            character1Anim.SetBool("isSick", true);
                            break;
                        case 1:
                            character2Anim.SetBool("isSick", true);
                            break;
                    }
                    
                }
                else
                {
                    switch(selectedChar){
                        case 0:
                            character1Anim.SetBool("isReady", true);
                            character1Anim.SetTrigger("Ready");
                            break;
                        case 1:
                            character2Anim.SetBool("isReady", true);
                            character2Anim.SetTrigger("Ready");
                            break;
                    }
                    
                }
                
                switch(randomEnemy){
                    case 0:
                        enemy1Anim.SetTrigger("Idle");
                        break;
                    case 1:
                        enemy1Anim.SetTrigger("Idle");
                        break;
                    case 2:
                        enemy1Anim.SetTrigger("Idle");
                        break;
                }

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
                                difficulty = "Normal";
                                break;
                            case "Normal":
                                difficulty = "Hard";
                                break;
                        }

                    }
                    else
                    {
                        switch (difficulty)
                        {
                            case "Normal":
                                difficulty = "Easy";
                                break;
                            case "Hard":
                                difficulty = "Normal";
                                break;
                        }
                    }
                }

                // Filter question list according to difficulty
                switch (difficulty)
                {
                    case "Easy":
                        randomQuestionNo = Random.Range(0, questionList_Filtered_Easy.Count);
                        qn = questionList_Filtered_Easy[randomQuestionNo].question;
                        o1 = questionList_Filtered_Easy[randomQuestionNo].option1;
                        o2 = questionList_Filtered_Easy[randomQuestionNo].option2;
                        o3 = questionList_Filtered_Easy[randomQuestionNo].option3;
                        o4 = questionList_Filtered_Easy[randomQuestionNo].option4;
                        answer = questionList_Filtered_Easy[randomQuestionNo].answer;
                        break;
                    case "Normal":
                        randomQuestionNo = Random.Range(0, questionList_Filtered_Normal.Count);
                        Debug.LogError("RANDOM QN = NORMAL " + randomQuestionNo.ToString());
                        qn = questionList_Filtered_Normal[randomQuestionNo].question;
                        o1 = questionList_Filtered_Normal[randomQuestionNo].option1;
                        o2 = questionList_Filtered_Normal[randomQuestionNo].option2;
                        o3 = questionList_Filtered_Normal[randomQuestionNo].option3;
                        o4 = questionList_Filtered_Normal[randomQuestionNo].option4;
                        answer = questionList_Filtered_Normal[randomQuestionNo].answer;
                        break;
                    case "Hard":
                        randomQuestionNo = Random.Range(0, questionList_Filtered_Hard.Count);
                        qn = questionList_Filtered_Hard[randomQuestionNo].question;
                        o1 = questionList_Filtered_Hard[randomQuestionNo].option1;
                        o2 = questionList_Filtered_Hard[randomQuestionNo].option2;
                        o3 = questionList_Filtered_Hard[randomQuestionNo].option3;
                        o4 = questionList_Filtered_Hard[randomQuestionNo].option4;
                        answer = questionList_Filtered_Hard[randomQuestionNo].answer;
                        break;
                }
                Debug.Log("------- Correct Answer = " + answer.ToString());
                difficultyTxt.text = "Difficulty: " + difficulty.ToString();
                questionTxt.text = qn;
                o1Text.text = "a. " + o1;
                o2Text.text = "b. " + o2;
                o3Text.text = "c. " + o3;
                o4Text.text = "d. " + o4;

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
        if (selectedOption == answer)
        {
            //==================
            correctAns += 1; // for report use
            //==========================

            switch(selectedChar){
                case 0:
                    character1Anim.SetTrigger("Stabbing");
                    break;
                case 1:
                    character2Anim.SetTrigger("Stabbing");
                    break;
            }

            switch(randomEnemy){
                case 0:
                    enemy1Anim.SetTrigger("Damage");
                    break;
                case 1:
                    enemy2Anim.SetTrigger("Damage");
                    break;
                case 2:
                    enemy3Anim.SetTrigger("Damage");
                    break;
            }

            isCorrect = true;

            int scoreGiven = 0;

            questionTxt.text = "Correct!";
            

            healthSystemEnemy.Damage(10);

            // Enemy HP is 0
            if (healthSystemEnemy.GetHealth() == 0)
            {

                switch(selectedChar){
                    case 0:
                        character1Anim.SetTrigger("Victory");
                        break;
                    case 1:
                        character2Anim.SetTrigger("Victory");
                        break;
                }

                switch(randomEnemy){
                    case 0:
                        enemy1Anim.SetTrigger("Down");
                        break;
                    case 1:
                        enemy2Anim.SetTrigger("Down");
                        break;
                    case 2:
                        enemy3Anim.SetTrigger("Down");
                        break;
                }


                // End Stage
                Invoke("EndStage", 1);
            }
            else if (healthSystemPlayer.GetHealth() == 10)
            {
                switch(selectedChar){
                    case 0:
                        character1Anim.SetBool("isSick", true);
                        break;
                    case 1:
                        character2Anim.SetBool("isSick", true);
                        break;
                }
                
            }
            else
            {
                switch(selectedChar){
                    case 0:
                        character1Anim.SetTrigger("Ready");
                        break;
                    case 1:
                        character2Anim.SetTrigger("Ready");
                        break;
                }
                
            }

            switch (difficulty)
            {
                case "Easy":
                    scoreGiven = 10;
                    break;
                case "Normal":
                    scoreGiven = 20;
                    break;
                case "Hard":
                    scoreGiven = 40;
                    break;
            }

            score = score + scoreGiven;
            scoreTxt.text = "Score: " + score.ToString();
            
            rightansNo++;
        }
        else
        {
            wrongAns += 1;  // for report use=========

            // Set no. of QN Wrong
            qnWrong += 1;

            switch(selectedChar){
                    case 0:
                        character1Anim.SetTrigger("Damage");
                        break;
                    case 1:
                        character2Anim.SetTrigger("Damage");
                        break;
                }

                switch(randomEnemy){
                    case 0:
                        enemy1Anim.SetTrigger("Swinging");
                        break;
                    case 1:
                        enemy2Anim.SetTrigger("Swinging");
                        break;
                    case 2:
                        enemy3Anim.SetTrigger("Swinging");
                        break;
                }


            
            

            isCorrect = false;

            questionTxt.text = "Wrong!";
            

            healthSystemPlayer.Damage(10);

            // Character HP is 0
            if (healthSystemPlayer.GetHealth() == 0)
            {
                switch(selectedChar){
                    case 0:
                        character1Anim.SetTrigger("Down");
                        break;
                    case 1:
                        character2Anim.SetTrigger("Down");
                        break;
                }

                switch(randomEnemy){
                    case 0:
                        enemy1Anim.SetTrigger("Idle");
                        break;
                    case 1:
                        enemy2Anim.SetTrigger("Idle");
                        break;
                    case 2:
                        enemy3Anim.SetTrigger("Idle");
                        break;
                }

                // Set Score to 0 (Stage Fail)
                score = 0;

                // End Stage
                Invoke("EndStage", 1);
            }
            else if (healthSystemPlayer.GetHealth() == 10)
            {
                switch(selectedChar){
                    case 0:
                        character1Anim.SetBool("isSick", true);
                        break;
                    case 1:
                        character2Anim.SetBool("isSick", true);
                        break;
                }

            }
            else
            {
                switch(selectedChar){
                    case 0:
                        character1Anim.SetBool("isReady", true);
                        break;
                    case 1:
                        character2Anim.SetBool("isReady", true);
                        break;
                }
            }

        }

        switch(randomEnemy){
                case 0:
                    enemy1Anim.SetTrigger("Idle");
                    break;
                case 1:
                    enemy2Anim.SetTrigger("Idle");
                    break;
                case 2:
                    enemy3Anim.SetTrigger("Idle");
                    break;
            }

        Debug.Log("Score = " + score.ToString());

        doUpdate = true;

        switch (difficulty)
        {
            case "Easy":
                questionList_Filtered_Easy.RemoveAt(randomQuestionNo);
                break;
            case "Normal":
                questionList_Filtered_Normal.RemoveAt(randomQuestionNo);
                break;
            case "Hard":
                questionList_Filtered_Hard.RemoveAt(randomQuestionNo);
                break;
        }
    }


    // Check whether pass or fail
    public void EndStage()
    {
        SaveReport(); //=============

        PlayerPrefs.SetInt("stageCorrectAns", rightansNo);
        int stagelevel = level - 1;
        PlayerPrefs.SetInt("stageQnsAttempt", stagelevel);
        

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
        if (PlayerPrefs.GetInt("Score") != 0)
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

    public void SaveReport() 
    {
                 FirebaseDatabase.DefaultInstance.GetReference("Report").Child(worldAndStage).GetValueAsync().ContinueWith(task => {
                       if (task.IsFaulted)
                       {
                           Debug.Log("HI, S =  what you have");
                           // Handle the error...
                       }
                       else if (task.IsCompleted)
                       {
                           DataSnapshot s = task.Result;

                           foreach(DataSnapshot node in s.Children)
                         {
                             Debug.Log(node.Key + ": " + node.Value);

                             if(node.Key == "Correct")
                             {
                                 correctAns = correctAns + int.Parse(node.Value.ToString());

                             }

                             if(node.Key == "Wrong")
                             {
                                 wrongAns = wrongAns + int.Parse(node.Value.ToString());
                             }
                         }
                         updateReport(correctAns, wrongAns);
                           // Do something with snapshot...
                       }
                   });
     
    }

    public void updateReport(int correctAns, int wrongAns)
    {
        double p = 0.0;
        reference.Child("Report").Child(worldAndStage).Child("Correct").SetValueAsync(correctAns);
        reference.Child("Report").Child(worldAndStage).Child("Wrong").SetValueAsync(wrongAns);
        reference.Child("Report").Child(worldAndStage).Child("Appear").SetValueAsync(correctAns + wrongAns);
        //=================
    }




}



