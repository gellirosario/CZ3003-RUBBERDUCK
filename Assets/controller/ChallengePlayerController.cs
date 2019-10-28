using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class ChallengePlayerController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;
    public InputField inputField;
    private bool isFirebaseInitialized = false;
    public Text messageTxt;
    private bool selected = false;
    //public InputField playerInput;
    // Start is called before the first frame update
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
    public void Challenge()
    {
        Debug.Log(inputField.text.Trim());
        //Debug.Log(string.Format("selected {0}:{1} ", selectIndex, characterList[selectIndex].characterName));
        //reference.Child("Player").Child(PlayerPrefs.GetString("UserID")).Child("characterID").SetValueAsync(selectIndex);
        //messageTxt.text = "Character change to " + characterList[selectIndex].characterName;
        //reference.Child("Users").Child("name").SetValueAsync(inputField.text.Trim());

        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task => 
        {
         if (task.IsFaulted)
         {
                Debug.Log("cant find");
         }
         else if (task.IsCompleted)
         {
                 DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Child("name").Value.ToString()+"yay");
                /*foreach (DataSnapshot user in snapshot.Children)
                {
                    //IDictionary dictUser = (IDictionary)user.Value ;
                    IDictionary dictUser = (IDictionary)user.Value;

                   if(dictUser["name"].ToString() == inputField.text.ToString())
                   {
                        //string uid = user.Key.ToString();
                        //Debug.Log("" + dictUser["name"] + " - " + dictUser["email"]+ user.Key);
                        //PlayerPrefs.SetString("challengedID", uid);
                        selected = true;
                        //SceneManager.LoadScene("ChallengeSelect");
                        //break;

                    }

                }*/
               if(selected)
                {
                    Debug.Log(" found");
                    
                }
                else { messageTxt.text = "Cannot find user " + inputField.text + "in firebase"; }
               
                

            }
     });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
