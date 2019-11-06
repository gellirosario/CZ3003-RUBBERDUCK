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
        StartCoroutine(DeleteAssignmentCode());
        //reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(deleteAssignmentInput.text).RemoveValueAsync();
    }

    private IEnumerator DeleteAssignmentCode()
    {
        yield return new WaitForEndOfFrame();

        /*FirebaseDatabase.DefaultInstance.GetReference("Assignment").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Assignment table");
            }
            else if (task.IsCompleted)
            {
                reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(PlayerPrefs.GetString("DeleteAssignmentCode")).RemoveValueAsync();
            }
        });*/

        reference.Child("Assignment").Child(PlayerPrefs.GetString("UserID")).Child(PlayerPrefs.GetString("DeleteAssignmentCode")).RemoveValueAsync();

    }
}
