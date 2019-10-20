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
    
    public List<Question> questionList = new List<Question>();
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
        /*
        for (int i = 0; i < 3; i++)
        {
            Question question = new Question(1, 1, 1, "Medium", "Test Question " + i, 1, "1", "2", "3", "4");
            string json = JsonUtility.ToJson(question);
            reference.Child("Questions").Child(i.ToString()).SetRawJsonValueAsync(json);
        }
        */
        
        GetQuestionsFromDatabase();
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

                
                foreach (DataSnapshot questionNode in snapshot.Children)
                {
                    //Debug.LogFormat("Key = {0}", questionNode.Key);  // "Key = questionNo"
                    var questionDict = (IDictionary <string, object>) questionNode.Value;
                    Question quest = new Question(questionDict);
                    questionList.Add(quest);
                }

                Debug.Log(questionList[0].question);
            }
        });
        
    }
    
}
