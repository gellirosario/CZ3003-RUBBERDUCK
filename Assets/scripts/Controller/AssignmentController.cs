using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;

public class AssignmentController : MonoBehaviour
{
    public static AssignmentController Instance { get; set; }

    public InputField createAssignmentInput;
    public InputField deleteAssignmentInput;
    public Text displayDeleteMessage;
    public string uid;

    private List<Question> questionList = new List<Question>();
    private List<string> assignmentNameList = new List<string>();
    private List<Assignment> assignmentQuestionList = new List<Assignment>();

    public GameObject popupSuccess, popupFailure;
    public Text idText, warningText, idText2, warningText2, feedbackText;
    public Assignment assignmentData { get; private set; }
    private DatabaseReference reference;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set up the Editor before calling into the realtime database.
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");

                // Get the root reference location of the database.
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /*Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set up the Editor before calling into the realtime database.
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");

                // Get the root reference location of the database.
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/

        /*// Retrieve Question List According to World, Stage and Difficulty
        print("Before loading questionlist count: " + questionList.Count);
        if (questionList.Count == 0)
        {
            questionList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(1,1);
        }*/
    }

    //toggles the popup window on assignment deletion
    public void TogglePopupSuccess(GameObject popupSuccess)
    {
        if (popupSuccess != null)
        {
            bool isActive = popupSuccess.activeSelf;
            popupSuccess.SetActive(!isActive);
        }
    }

    public void TogglePopupFailure(GameObject popupFailure)
    {
        if (popupFailure != null)
        {
            bool isActive = popupFailure.activeSelf;
            popupFailure.SetActive(!isActive);
        }
    }


    public void SaveAssignment(List<int> qnsList)
    {
        print("Enter save assignment");
        print("Before loading questionlist count: " + questionList.Count);
        //questionList = QuestionLoader.Instance.FilterQuestionsFromAssignment();
        List<Question> allQns = new List<Question>();
        allQns = QuestionLoader.Instance.passmeAllQuestions();

        for (int i = 0; i < qnsList.Count; i++)
        {
            for (int j = 0; j < allQns.Count; j++)
            {
                if (qnsList[i] == allQns[j].qnID)
                {
                    questionList.Add(allQns[j]);
                    //Debug.Log(questionList_All[j].qnID + " Added");
                }
            }

        }

        print("After loading questionlist count: " + questionList.Count);
        print("Assignment Name Inputted Text: " + createAssignmentInput.text.ToString());
        DatabaseReference referenceRef = reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(createAssignmentInput.text);
        print("Before looping the qnsList");
        for (int i = 0; i < questionList.Count; i++)
        {
            print("Inside looping the qnsList");
            Assignment assignment = new Assignment(createAssignmentInput.text, questionList[i].qnID, questionList[i].world, questionList[i].stage, questionList[i].difficulty,
                questionList[i].question, questionList[i].answer, questionList[i].option1, questionList[i].option2, questionList[i].option3, questionList[i].option4);
            string json = JsonUtility.ToJson(assignment);
            //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(assignment.assignmentName).SetRawJsonValueAsync(json);
            referenceRef.Child((i + 1).ToString()).SetRawJsonValueAsync(json);
        }

        // Clear the number of questions to 0 after adding to firebase
        questionList.Clear();

        // Clear the Input text on the screen after adding to firebase
        createAssignmentInput.text = "";
    }


    public void LoadAssignment()
    {
        Debug.LogFormat("----HERE---");
        uid = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----ASSIGNMENT INFO ID---" + uid);

        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Assignment table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot userid in snapshot.Children)
                {
                    Debug.Log("USER ID OF FIREBASE:" + userid.Key);
                    Debug.Log("USER ID OF CURRENT USER:" + uid);
                    if (userid.Key == uid)
                    {
                        foreach (DataSnapshot assignmentname in userid.Children)
                        {
                            print("reach here");
                            Debug.LogFormat("ASSIGNMENT NAME ---- Key = {0}", assignmentname.Key);
                            foreach (DataSnapshot assignmentquestionid in assignmentname.Children)
                            {
                                Debug.LogFormat("ASSIGNMENT QUESTION ID ---- Key = {0}", assignmentquestionid.Key);
                                //load assignment data into assignment object
                                var assignmentQnIDDict = (IDictionary<string, object>)assignmentquestionid.Value;

                                foreach (KeyValuePair<string, object> kvp in assignmentQnIDDict)
                                {
                                    Debug.LogFormat("ASSIGNMENT QUESTION DETAILS ---- Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                                }

                                assignmentData = new Assignment(assignmentQnIDDict);
                                Debug.Log("Assignment Name: " + assignmentData.assignmentName);
                                // adding list of questions in assignment names
                                assignmentQuestionList.Add(assignmentData);
                                //assignmentList.Add(assignmentData);
                                print("Assignment list count: " + assignmentQuestionList.Count);
                            }

                            // adding list of assignment names
                            assignmentNameList.Add(assignmentname.Key.ToString());
                        }
                        break;
                    }
                }
            }
            print("Current list of assignment names in assignment : " + assignmentNameList.Count);
            PlayerPrefs.SetInt("AssignmentNameListCount", assignmentNameList.Count);
            for (var i = 0; i < assignmentNameList.Count; i++)
            {
                PlayerPrefs.SetString("AssignmentNameList" + i, assignmentNameList[i]);
            }

            print("Current list of questions in assignment name: " + assignmentQuestionList.Count);
            PlayerPrefs.SetInt("AssignmentQuestionListCount", assignmentQuestionList.Count);
            for (var i = 0; i < assignmentQuestionList.Count; i++)
            {
                PlayerPrefs.SetString("AssignmentList(assignmentName)" + i, assignmentQuestionList[i].assignmentName);
                PlayerPrefs.SetInt("AssignmentList(qnID)" + i, assignmentQuestionList[i].qnID);
                PlayerPrefs.SetInt("AssignmentList(world)" + i, assignmentQuestionList[i].world);
                PlayerPrefs.SetInt("AssignmentList(stage)" + i, assignmentQuestionList[i].stage);
                PlayerPrefs.SetString("AssignmentList(difficulty)" + i, assignmentQuestionList[i].difficulty);
                PlayerPrefs.SetString("AssignmentList(question)" + i, assignmentQuestionList[i].question);
                PlayerPrefs.SetInt("AssignmentList(answer)" + i, assignmentQuestionList[i].answer);
                PlayerPrefs.SetString("AssignmentList(option1)" + i, assignmentQuestionList[i].option1);
                PlayerPrefs.SetString("AssignmentList(option2)" + i, assignmentQuestionList[i].option2);
                PlayerPrefs.SetString("AssignmentList(option3)" + i, assignmentQuestionList[i].option3);
                PlayerPrefs.SetString("AssignmentList(option4)" + i, assignmentQuestionList[i].option4);
            }
        });
    }

    int checkcode = 0;

    public void DeleteAssignment()
    {
        Debug.LogError("===test====+" + deleteAssignmentInput.text.ToString());
        PlayerPrefs.SetString("DeleteAssignmentName", deleteAssignmentInput.text.ToString());
        uid = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----ASSIGNMENT INFO ID---" + uid);
        

        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Assignment table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot assignmentNode in snapshot.Children)
                {
                    Debug.Log("USER ID OF FIREBASE:" + assignmentNode.Key);
                    Debug.Log("USER ID OF CURRENT USER:" + uid);
                    if (assignmentNode.Key == uid)
                    {
                        var assignmentDict = (IDictionary<string, object>)assignmentNode.Value;
                        

                        foreach (var key in assignmentDict.Keys) // loop through keys
                        {
                            Debug.Log("ASSIGNMENT NAME:" + key);
                            Debug.Log("Inputted Delete Assignment Text:" + deleteAssignmentInput.text);
                            if (key.ToString() == deleteAssignmentInput.text.ToString())
                            {
                                idText.text = "Assignment Name: " + deleteAssignmentInput.text.ToString();
                                
                                //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(deleteAssignmentInput.text).RemoveValueAsync();
                                warningText.text = "Click Confirm to delete " + deleteAssignmentInput.text;
                                
                                checkcode += 1;
                                
                                break;
                            }
                        }
                    }
                }
                DisplayPopUpMsg();
            }
        });
    }

    public void DisplayPopUpMsg()
    {
        print("============= CHECK CODE " + checkcode);
        if (checkcode == 0)
        {
            idText2.text = "Assignment Name: " + deleteAssignmentInput.text.ToString();
            warningText2.text = "";
            TogglePopupFailure(popupFailure);
        }
        else
        {
            TogglePopupSuccess(popupSuccess);
            checkcode = 0;
        }

    }
}