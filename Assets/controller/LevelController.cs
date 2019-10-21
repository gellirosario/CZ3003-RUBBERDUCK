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
    
    public List<Question> questionList = new List<Question>();
    private bool doUpdate = false;
    
    private string world;
    private string stage;
    private int score;
    
    public void Awake()
    {
        levelTxt.text = "Level " + "";
        questionTxt.text = "";
        o1Text.text = "";
        o2Text.text = "";
        o3Text.text = "";
        o4Text.text = "";

        if (questionList.Count == 0)
        {
            questionList = QuestionLoader.Instance.questionList_All;
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
    
    private void Update() {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (doUpdate == true && questionList != null)
        {
            int qnId = int.Parse(questionList[0].qnID.ToString());
        
            levelTxt.text = "LEVEL " + qnId.ToString();
            questionTxt.text = questionList[0].question;
            o1Text.text = questionList[0].option1.ToString();
            o2Text.text = questionList[0].option2.ToString();
            o3Text.text = questionList[0].option3.ToString();
            o4Text.text  = questionList[0].option4.ToString();

            doUpdate = false;
        }
    }

    private void OnClick_Option1()
    {
        
    }
    
    private void OnClick_Option2()
    {
        
    }
    
    private void OnClick_Option3()
    {
        
    }
    
    private void OnClick_Option4()
    {
        
    }
}
    
    

