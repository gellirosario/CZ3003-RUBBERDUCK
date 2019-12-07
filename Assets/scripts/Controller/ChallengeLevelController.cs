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

    private Color colorGreen = new Color(0, 198, 0);
    private Color colorRed = new Color(255, 0, 0);

    public Animator character1Anim;
    public Animator character2Anim;
    public Animator enemy1Anim;
    public Animator enemy2Anim;
    public Animator enemy3Anim;

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
        // Select the chosen character by the character id
        selectedChar = PlayerPrefs.GetInt("CharacterID");

        // Randomly select 1 enemy from the 3 available enemies (index range 0 to 2)
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
            Debug.Log("Questions Left: " + questionsLeft);

            // Set the randomized enemy to be idle
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
        questionsLeft--;
        if (selectedOption == answer) //if correct
        {
            // Character initiates an attack on enemy
            switch (selectedChar)
            {
                case 0:
                    character1Anim.SetTrigger("Stabbing");
                    break;
                case 1:
                    character2Anim.SetTrigger("Stabbing");
                    break;
            }

            // Enemy receives a damage from character
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
                // Set the character to be ready
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
        else //if wrong
        {
            // Set no. of QN Wrong
            qnWrong += 1;

            // Character receives a damage from enemy
            switch (selectedChar)
            {
                case 0:
                    character1Anim.SetTrigger("Damage");
                    break;
                case 1:
                    character2Anim.SetTrigger("Damage");
                    break;
            }

            // Enemy initiates an attack on character
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

            if (questionsLeft == 0)
            { 
                // End Stage
                Invoke("EndStage", 1);
            }

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


    // End of Challenge
    public void EndStage()
    {

        PlayerPrefs.SetInt("stageCorrectAns", rightansNo);
        int stagelevel = level;
        PlayerPrefs.SetInt("stageQnsAttempt", stagelevel);

        PlayerPrefs.SetInt("Score", score);
        SavePlayerScore();

        QuestionLoader.Instance.challenge = null;
        questionList = null;

        Debug.Log("Preferences set: Score - " + score.ToString());
        SceneManager.LoadScene("ChallengeEnd");
    }

    //save player score to challenge db
    public void SavePlayerScore()
    {
        //retrieve challenge object from questionloader
        Challenge challengeToUpload = QuestionLoader.Instance.challenge;

        challengeToUpload.addPlayerAndScore(PlayerPrefs.GetString("UserID"), ProfileLoader.userData.name, PlayerPrefs.GetInt("Score"));

        string json = JsonUtility.ToJson(challengeToUpload);
        reference.Child("Challenges").Child(challengeToUpload.challengeId).SetRawJsonValueAsync(json);
    }
}



