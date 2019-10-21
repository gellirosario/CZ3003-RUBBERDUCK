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
    private bool doUpdate = false;
    
    public void Start()
    {
        levelTxt.text = "";
        questionTxt.text = "";
        o1Text.text = "";
        o2Text.text = "";
        o3Text.text = "";
        o4Text.text = "";
        
        o4Text = o4Text.GetComponent<Text>();
        Debug.Log(o4Text != null);
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
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
                
                Debug.Log(questionList[1].question);
                
                Debug.Log(questionList[2].question);

                doUpdate = true;

            }
        });
        
        
    }
    
    private void Update()
    {
        
        if (questionList == null)
        {
            GetQuestionsFromDatabase();
        }
        Debug.Log(questionList[0].qnID);

        int qnId = int.Parse(questionList[0].qnID.ToString());
        levelTxt.text = "Level " + qnId+1;
        questionTxt.GetComponent<Text>().text = questionList[0].question;
        o1Text.text = questionList[0].option1.ToString();
        o2Text.text = questionList[0].option2.ToString();
        o3Text.text = questionList[0].option3.ToString();
        o4Text.text = questionList[0].option4.ToString();

        doUpdate = false;
    }
}
    
    

