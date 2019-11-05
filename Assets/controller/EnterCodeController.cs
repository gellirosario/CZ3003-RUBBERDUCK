using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
public class EnterCodeController : MonoBehaviour
{
    // Start is called before the first frame update

    private FirebaseApp app;
    private DatabaseReference reference;
    public InputField inputField;
    private bool isFirebaseInitialized = false;
    public Text messageTxt;
    private string cid = null;
    private string aid = null;
    private int selected = 0;
    

    public GameObject popup, popup2, leaderboard;
    public Text idText, idText2;

    void Awake()
    {
        messageTxt.text = "";
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

    void Start()
    {

    }

    public void TogglePopup(GameObject popup)
    {
        if (popup != null)
        {
            bool isActive = popup.activeSelf;
            popup.SetActive(!isActive);
        }
    }

    public void TogglePopupAssignmentPanel(GameObject popup2)
    {
        if (popup2 != null)
        {
            bool isActive = popup2.activeSelf;
            popup2.SetActive(!isActive);
        }
    }

    public void SearchCode()
    {
        selected = 0;
        searchChallenge();

        


    }

    private void searchChallenge()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Challenges").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("cant find");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Child("name").Value.ToString()+"yay");
                foreach (DataSnapshot challenge in snapshot.Children)
                {

                    //var dictUser = (IDictionary<string, object>)user.Value;

                    //Debug.Log("" + dictUser["challengeId"] + " - " + dictUser["challengeId"] + user.Key);
                    if (challenge.Key == inputField.text.ToString())
                    {
                        cid = challenge.Key.ToString();
                        //Debug.Log("" + dictUser["name"] + " - " + dictUser["email"]+ user.Key);
                        Debug.Log(cid+"-cid");
                        PlayerPrefs.SetString("challengeID",cid);
                        
                        selected += 1;
                        Debug.Log(" found in challenges");
                        Debug.Log(selected);
                        //load challenge from db
                        messageTxt.text = "";
                        string challengeData = challenge.GetRawJsonValue();

                        //load challenge into questionloader for further processing
                        QuestionLoader.Instance.challenge = JsonUtility.FromJson<Challenge>(challengeData);

                        idText.text = "ID: " + cid;
                        TogglePopup(popup);
                    }
                }
               
            }
        });
        searchAssignment();

    }
    private void searchAssignment()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Assignment2").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("cant find");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Child("name").Value.ToString()+"yay");
                foreach (DataSnapshot assignment in snapshot.Children)
                {

                    //var dictUser = (IDictionary<string, object>)user.Value;

                    //Debug.Log("" + dictUser["challengeId"] + " - " + dictUser["challengeId"] + user.Key);
                    if (assignment.Key == inputField.text.ToString())
                    {
                        messageTxt.text = "";
                        aid = assignment.Key.ToString();
                        print("ASSIGNMENT ID IS: " + aid);
                        //Debug.Log("" + dictUser["name"] + " - " + dictUser["email"]+ user.Key);
                        //Debug.Log(uid);
                        PlayerPrefs.SetString("assignmentID", aid);
                        //selected2 = true;
                        selected += 1;
                        Debug.Log(" found in assignment");
                        //break;

                        //load assignment from db
                        string assignmentData = assignment.GetRawJsonValue();

                        //load challenge into questionloader for further processing
                        QuestionLoader.Instance.assignment = JsonUtility.FromJson<Assignment>(assignmentData);

                        idText2.text = "ID: " + aid;
                        TogglePopupAssignmentPanel(popup2);
                    }
                }
                checking();
            }
        });
        
    }
    private void checking()
    {
        if (selected == 0)
        {
            messageTxt.text = "Cannot find code '" + inputField.text + "'";
            
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
