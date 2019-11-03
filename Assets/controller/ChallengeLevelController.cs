using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ChallengeLevelController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;

    public Text questionTxt, levelTxt, o1Text, o2Text, o3Text, o4Text;
    public Button option1Btn, option2Btn, option3Btn, option4Btn;

    public Text scoreTxt;
    public Text difficultyTxt;

    private List<Question> questionList = new List<Question>();

    private string qn, o1, o2, o3, o4; // For the UI
    private int answer; // For the UI

    private bool doUpdate = false;
    private bool isCorrect = false;
    private bool isChecked = false;

    private string difficulty;
    private int level;
    private int score;
    private int randomQuestionNo;
    private int qnWrong;
    private int rightansNo = 0;

    private int questionsLeft;

    private string worldAndStage;  //ss ==
    private int appear = 0;//ss ==
    private int correctAns = 0;//ss ==
    private int wrongAns = 0;//ss ==

    public Challenge challenge;

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



    public void Start()
    {
        difficultyTxt.text = "Difficulty: ";
        scoreTxt.text = "Score: 0";
    }

    public void Awake()
    {
        selectedChar = PlayerPrefs.GetInt("CharacterID");
        randomEnemy = Random.Range(0, 2);

        switch (selectedChar)
        {
            case 0:
                charSprite2.GetComponent<Renderer>().enabled = false;
                break;
            case 1:
                charSprite1.GetComponent<Renderer>().enabled = false;
                break;
        }

        switch (randomEnemy)
        {
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

        level = 0;
        score = 0;
        qnWrong = 0;

        if (questionList.Count == 0)
        {
            Debug.Log("Retrieving challenge questions...");

            // Retrieve Question List According to World and Stage
            questionList = QuestionLoader.Instance.FilterQuestionsFromChallenge();

            // for report use ============= ss
            worldAndStage = "w" + questionList[0].world +
                            "s" + questionList[0].stage;
            //==========================
            questionsLeft = questionList.Count;

            foreach (Question q in questionList)
            {
                Debug.Log(q.qnID);
            }
        }

        doUpdate = true;

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
        if (doUpdate == true && questionList != null && questionsLeft > 0)
        {

            switch (randomEnemy)
            {
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

            qn = questionList[level - 1].question;
            o1 = questionList[level - 1].option1;
            o2 = questionList[level - 1].option2;
            o3 = questionList[level - 1].option3;
            o4 = questionList[level - 1].option4;
            answer = questionList[level - 1].answer;
            difficulty = questionList[level - 1].difficulty;

            // Set question and options
            Debug.Log("------- Correct Answer = " + answer.ToString());
            difficultyTxt.text = "Difficulty: " + difficulty.ToString();
            questionTxt.text = qn;
            o1Text.text = "a. " + o1;
            o2Text.text = "b. " + o2;
            o3Text.text = "c. " + o3;
            o4Text.text = "d. " + o4;

            doUpdate = false; // Set to false until the player answered
        }

    }

    // Check selection option
    public void CheckAnswer(int selectedOption)
    {

        //decrement qn counter
        questionsLeft--;

        if (selectedOption == answer)
        {
            //==================
            correctAns += 1; // for report use
            //==========================

            switch (selectedChar)
            {
                case 0:
                    character1Anim.SetTrigger("Stabbing");
                    break;
                case 1:
                    character2Anim.SetTrigger("Stabbing");
                    break;
            }

            switch (randomEnemy)
            {
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


            // no more questions left
            if (questionsLeft == 0)
            {

                switch (selectedChar)
                {
                    case 0:
                        character1Anim.SetTrigger("Victory");
                        break;
                    case 1:
                        character2Anim.SetTrigger("Victory");
                        break;
                }

                switch (randomEnemy)
                {
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
            else
            {
                switch (selectedChar)
                {
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

            switch (selectedChar)
            {
                case 0:
                    character1Anim.SetTrigger("Damage");
                    break;
                case 1:
                    character2Anim.SetTrigger("Damage");
                    break;
            }

            switch (randomEnemy)
            {
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

        }

        switch (randomEnemy)
        {
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
    }


    // Check whether pass or fail
    public void EndStage()
    {
        SaveReport(); //=============

        PlayerPrefs.SetInt("stageCorrectAns", rightansNo);
        int stagelevel = level - 1;
        PlayerPrefs.SetInt("stageQnsAttempt", stagelevel);


        QuestionLoader.Instance.challenge = null;
        questionList = null;

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

                foreach (DataSnapshot node in s.Children)
                {
                    Debug.Log(node.Key + ": " + node.Value);

                    if (node.Key == "Correct")
                    {
                        correctAns = correctAns + int.Parse(node.Value.ToString());

                    }

                    if (node.Key == "Wrong")
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



