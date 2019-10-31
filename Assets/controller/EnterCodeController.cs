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
    private string uid = null;
    private bool selected = false;
    private bool selected2 = false;
    void Start()
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
    public void SearchCode()
    {
        searchChallenge();
        searchAssignment();
        
         if(selected== false)
        { 
         //messageTxt.text = "Cannot find code " + inputField.text + " in firebase";
        }
        

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
                        uid = challenge.Key.ToString();
                        //Debug.Log("" + dictUser["name"] + " - " + dictUser["email"]+ user.Key);
                        //Debug.Log(uid);
                        //PlayerPrefs.SetString("challengedID", uid);
                        selected = true;
                        Debug.Log(" found in challenges");
                        //break;
                        
                    }
                }
               
            }
        });
        //return selected;
    }
    private void searchAssignment()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
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
                        uid = assignment.Key.ToString();
                        //Debug.Log("" + dictUser["name"] + " - " + dictUser["email"]+ user.Key);
                        //Debug.Log(uid);
                        //PlayerPrefs.SetString("challengedID", uid);
                        //selected2 = true;
                        selected = true;
                        Debug.Log(" found in assignment");
                        //break;
                    }
                }
               
            }
        });
        //return selected2;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
