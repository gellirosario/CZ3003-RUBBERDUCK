using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LeaderboardLoader : MonoBehaviour
{
    public static LeaderboardLoader Instance { get; set; }

    private FirebaseApp app;
    private DatabaseReference reference;
	public string id;
    public Player playerData { get; private set; }
    //public User userData { get; private set; }

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
		//LoadUserData();
        LoadPlayerData();
	}


    /*
    private void LoadUserData()
    {
		Debug.LogFormat("----HERE---");
        id = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----USER INFO ID---" +id);

        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Users table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot userNode in snapshot.Children)
                {
                    if(userNode.Key == id)
                    {
                        //load user data into player object
                        var userDict = (IDictionary<string, object>)userNode.Value;
                        userData = new User(userDict);
                    }
                }
                Debug.Log(userData.userID);
                Debug.Log(userData.userType);
                Debug.Log(userData.name);
                Debug.Log(userData.email);
            }

        });

      
    }*/


    private void LoadPlayerData()
    {
        Debug.LogFormat("----HERE---");
        id = PlayerPrefs.GetString("UserID");
        Debug.LogFormat("----USER INFO ID---" + id);

        FirebaseDatabase.DefaultInstance.GetReference("Player").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error in data retrieval from Player table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot playerNode in snapshot.Children)
                {
                    if (playerNode.Key == id)
                    {
                        //load player data into player object
                        var playerDict = (IDictionary<string, object>)playerNode.Value;

						//Debug.LogFormat("Key = {0}, Value = {1}", playerNode.Key, playerNode.Value);
                        foreach (KeyValuePair<string, object> kvp in playerDict)
                        {
                            Debug.LogFormat("PLAYER ---- Key = {0}, Value = {1}", kvp.Key, kvp.Value);
							if(kvp.Key == "mastery")
							{
								var masteryDict = (IDictionary<string, object>)playerDict["mastery"];
								foreach (KeyValuePair<string, object> kvp1 in masteryDict)
                        		{
                            		Debug.LogFormat("MASTERY ---- Key = {0}, Value = {1}", kvp1.Key, kvp1.Value);
                        		}
								playerData = new Player(playerDict, masteryDict);
							}
							
                        }
                        
                    }
                }
                Debug.Log(playerData.characterID);
                Debug.Log(playerData.mastery);
                Debug.Log(playerData.totalPoints);
                Debug.Log(playerData.totalQnAnswered);
            }

        });
    }
}
