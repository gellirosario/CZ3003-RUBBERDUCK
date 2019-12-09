using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;


public class DeleteAssignmentQuestion : MonoBehaviour
{
    private DatabaseReference reference;
    private GameObject popupSuccess;
    public Text feedbackText;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
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
    }

    public void ConfirmDelete()
    {
        StartCoroutine(DeleteAssignmentName());
    }

    private IEnumerator DeleteAssignmentName()
    {
        yield return new WaitForEndOfFrame();

        Debug.LogFormat("DELETE " + PlayerPrefs.GetString("DeleteAssignmentName"));
        reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(PlayerPrefs.GetString("DeleteAssignmentName")).RemoveValueAsync();
        feedbackText.text = PlayerPrefs.GetString("DeleteAssignmentName") + " Successfully Deleted";

        /*Debug.LogFormat("DELETE " + PlayerPrefs.GetString("DeleteAssignmentNameViaDropdown"));
        reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(PlayerPrefs.GetString("DeleteAssignmentNameViaDropdown")).RemoveValueAsync();
        feedbackText.text = PlayerPrefs.GetString("DeleteAssignmentNameViaDropdown") + " Successfully Deleted";*/
    }
}
