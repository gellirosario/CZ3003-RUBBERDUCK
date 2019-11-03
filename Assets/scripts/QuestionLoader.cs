using System.Collections;
using System.Collections.Generic;
using System.IO;   
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using SimpleJSON;

public class QuestionLoader : MonoBehaviour
{
    public static QuestionLoader Instance { get; set; }
    
    private FirebaseApp app;
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;

    public List<Question> questionList_All = new List<Question>();

    public Challenge challenge;
    
    private string dataFileName = "QuestionsList.json"; // Json file in StreamingAssets folder
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
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
    
    private void AddQuestionData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFileName);

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string jsonString = File.ReadAllText(filePath);
            JSONNode data = JSON.Parse(jsonString);
            Debug.LogError("=ADD QUESTION=");
            
            foreach(JSONNode record in data["Questions"])
            {
                Question question = new Question(record["id"].AsInt, record["world"].AsInt, record["stage"].AsInt,
                    record["difficulty"].Value, record["question"].Value, record["answer"].AsInt,
                    record["option1"].Value, record["option2"].Value, record["option3"].Value, record["option4"].Value);
                string json = JsonUtility.ToJson(question);
                reference.Child("Questions").Child(record["id"].Value).SetRawJsonValueAsync(json);
            }
        }
        else
        {
            Debug.LogError("Cannot load question data!");
        }
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
                
                if (questionList_All.Count == 0)
                {
                    AddQuestionData();
                }

            }
        });

    }

    public List<Question> FilterQuestionsFromChallenge()
    {
        List<Question> questionList = new List<Question>();

        for (int i = 0; i < this.challenge.challengeQns.Count; i++)
        {
            for (int j = 0; j < questionList_All.Count;j++)
            {
                if (this.challenge.challengeQns[i] == questionList_All[j].qnID)
                {
                    questionList.Add(questionList_All[j]);
                    //Debug.Log(questionList_All[j].qnID + " Added");
                }
            }

        }
        return questionList;
    }

    public List<Question> FilterQuestionsByWorldAndStage(int world, int stage)
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

    public List<Question> FilterQuestionsListByDifficulty(List<Question> questionList, string difficulty)
    {
        List<Question> questionList_Filtered = new List<Question>();
                
        for (int i = 0; i < questionList.Count; i++)
        {
            if (questionList[i].difficulty == difficulty)
            {
                questionList_Filtered.Add(questionList[i]);
            }
        }

        return questionList_Filtered;
    }
}
