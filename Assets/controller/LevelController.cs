using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Proyecto26;

public class LevelController : MonoBehaviour
{
    private FirebaseApp app;
    private bool isFirebaseInitialized = false;
    private DatabaseReference reference;
    
    public Text questionTxt, levelTxt, o1Text, o2Text, o3Text, o4Text;
    public Button option1Btn, option2Btn, option3Btn, option4Btn;
    
    ArrayList questionList = new ArrayList();
    Question question = new Question();
    //private Question[] questionList;
    private static int levelNo = 0;
    
    public void Start()
    {
        levelTxt.text = "";
        questionTxt.text = "";
        o1Text.text = "";
        o2Text.text = "";
        o3Text.text = "";
        o4Text.text = "";
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                AddQuestionToDatabase();
                GetQuestionsFromDatabase();
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    // Testing purposes
    private void AddQuestionToDatabase()
    {
        for (int i = 0; i < 3; i++)
        {
            Question question = new Question(1, 1, "Medium", "Test Question " + i, 1, "1", "2", "3", "4");
            string json = JsonUtility.ToJson(question);
            reference.Child("Questions").Child("World").Child("1").Child("Stage").Child("1").SetRawJsonValueAsync(json);
        }
    }

    private void GetQuestionsFromDatabase()
    {
        Firebase.Database.FirebaseDatabase dbInstance = Firebase.Database.FirebaseDatabase.DefaultInstance;
        dbInstance.GetReference("Questions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
                Debug.Log("Error in data retrieval");
            }
            else if (task.IsCompleted) {
                Debug.Log("Received values for Questions.");
                
                DataSnapshot snapshot = task.Result;
                foreach (var questions in snapshot.Children){
                    levelTxt.text = "Level " + levelNo;
                    questionList.Insert(1,questions);
                    //option1Btn.GetComponentInChildren(Text).text = dictQuestions["option1"];
                    //option2Btn.GetComponentInChildren(Text).text = dictQuestions["option2"];
                    //option3Btn.GetComponentInChildren(Text).text = dictQuestions["option3"];
                    //option4Btn.GetComponentInChildren(Text).text = dictQuestions["option4"];
                }
            }
        });
        Debug.Log(questionList);
        //questionTxt.text = (Question) questionList[0].getQuestion();
    }
    
}
