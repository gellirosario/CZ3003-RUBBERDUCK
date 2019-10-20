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
    
    public Text questionTxt, levelTxt;
    public Button option1Btn, option2Btn, option3Btn, option4Btn;
    
    Question question = new Question();
    private Question[] questionList;
    
    public void Start()
    {
        TestRestClient();
        /*
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                AddQuestionToDatabase();
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/

    }

    // Testing purposes
    private void AddQuestionToDatabase()
    {
        for (int i = 0; i < 10; i++)
        {
            Question question = new Question(1, 1, "Medium", "Test Question " + i, 1, "1", "2", "3", "4");
            string json = JsonUtility.ToJson(question);
            reference.Child("questions").Child(i.ToString()).SetRawJsonValueAsync(json);
        }

        GetQuestionsFromDatabase();
    }

    private void GetQuestionsFromDatabase()
    {
        Firebase.Database.FirebaseDatabase dbInstance = Firebase.Database.FirebaseDatabase.DefaultInstance;
        dbInstance.GetReference("questions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach ( DataSnapshot questions in snapshot.Children){
                    IDictionary dictQuestions = (IDictionary)questions.Value;
                    Debug.Log ("" + dictQuestions["world"] + " - " + dictQuestions["question"]);
                    //levelTxt.text = "Level " + dictQuestions["id"];
                    //questionTxt.text = dictQuestions["questions"].k;
                    //option1Btn.GetComponentInChildren(Text).text = dictQuestions["option1"];
                    //option2Btn.GetComponentInChildren(Text).text = dictQuestions["option2"];
                    //option3Btn.GetComponentInChildren(Text).text = dictQuestions["option3"];
                    //option4Btn.GetComponentInChildren(Text).text = dictQuestions["option4"];
                }
            }
        });
    }

    private void TestRestClient()
    {
        for (int i = 0; i < 10; i++)
        {
            Question question = new Question(1, 1, "Medium", "Test Question " + i, 1, "1", "2", "3", "4");
            string json = JsonUtility.ToJson(question);
            RestClient.Put("https://teamrubberduck-1420e.firebaseio.com/"+ "questions/" + i + ".json", question);
        }

        RetrieveFromDatabase();
    }
    
    private void RetrieveFromDatabase()
    {
        for (int i = 0; i < 10; i++)
        {
            RestClient.Get<Question>("https://teamrubberduck-1420e.firebaseio.com/" + "questions/" + i + ".json").Then(response =>
            {
                questionList[i] = response;
            });
        }


        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log (questionList[0].getQuestion());
        questionTxt.text = questionList[0].getQuestion();
    }
}
