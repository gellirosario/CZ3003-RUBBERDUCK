using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ProfileLoader : MonoBehaviour
{
    public static ProfileLoader Instance { get; set; }

    private FirebaseApp app;
    private DatabaseReference reference;
	public string id;

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

	public void Start(){
		LoadUserData();
	}



    private void LoadUserData()
    {
		Debug.LogFormat("----HERE---");
        id = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----USER INFO ID---" +id);

        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;
                //to do the actual loading

            }

        });

      
    }
}
