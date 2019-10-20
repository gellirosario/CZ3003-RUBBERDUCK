using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LevelController : MonoBehaviour
{
    private List<Question> questionList = new List<Question>();
    private Firebase.FirebaseApp app;
    private bool isFirebaseInitialized = false;
    private DatabaseReference reference;
    
    public void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
                
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    
    // Initialize the Firebase database
    
    protected virtual void InitializeFirebase() {
        
        app.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
        
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        UnityEngine.Debug.LogError("Test");
        writeTestQuestionPool();
        isFirebaseInitialized = true;
    }

    // Testing purposes
    private void writeTestQuestionPool() {
        
        //for (int i = 0; i < 10; i++)
        //{
            AddQuestionToDatabase("1");
        //}
        
        
    }

    private void AddQuestionToDatabase(string questionID)
    {
        string difficulty;
        if (int.Parse(questionID) % 2 == 0)
        {
            difficulty = "Medium";
        }
        else
        {
            difficulty = "Easy";
        }
        
        Question question = new Question(1,1,difficulty,"Test Question " + questionID,1,"1","2","3","4");
        questionList.Add(question);
        string json = JsonUtility.ToJson(question);

        FirebaseDatabase.DefaultInstance.RootReference.Child("questions").Child(questionID).SetValueAsync(json);
    }
}
