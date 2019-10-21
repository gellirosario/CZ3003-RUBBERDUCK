using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class QuestionLoader : MonoBehaviour
{
    public static QuestionLoader Instance { get; set; }
    
    private FirebaseApp app;
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;

    public List<Question> questionList_All = new List<Question>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                GetAllQuestionsFromDatabase();

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
        
        GetAllQuestionsFromDatabase();
    }

    private void GetAllQuestionsFromDatabase()
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
                    questionList_All.Add(quest);
                }

                Debug.Log(questionList_All[0].question);
                
                Debug.Log(questionList_All[1].question);
                
                Debug.Log(questionList_All[2].question);
            }
        });

    }

    private List<Question> FilterQuestionsByWorldAndStage(int world, int stage)
    {
        List<Question> questionList_Filtered = new List<Question>();
        
        for (int i = 0; i < questionList_All.Count; i++)
        {
            if (questionList_All[i].world == world && questionList_All[i].stage == stage)
            {
                questionList_Filtered.Add(questionList_All[i]);
            }
        }

        return questionList_Filtered;
    }

    private List<Question> FilterQuestionsListByDifficulty(List<Question> questionList, string difficulty)
    {
        List<Question> questionList_Filtered = new List<Question>();
                
        for (int i = 0; i < questionList.Count; i++)
        {
            if (questionList[i].difficulty == difficulty)
            {
                questionList_Filtered.Add(questionList_All[i]);
            }
        }

        return questionList_Filtered;
    }
}
