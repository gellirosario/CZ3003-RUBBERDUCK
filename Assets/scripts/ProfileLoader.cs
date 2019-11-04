using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ProfileLoader : MonoBehaviour
{
    public static ProfileLoader Instance { get; set; }

    private FirebaseApp app;
    private DatabaseReference reference;
    public string id;
    public static Player playerData { get; private set; }
    public static User userData { get; private set; }

    /*----------leaderBoard-----------*/
    public Player topPlayerData { get; private set; }
    public User topUserData { get; private set; }

    public static List<Player> leaderboard = new List<Player>();
    public static List<User> playerName = new List<User>();

    private ThreadDispatcher dispatcher;

    public GameObject loadingScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            //show loading screen on first load
            ShowLoadingScreen();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
        }

        dispatcher = new ThreadDispatcher();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
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

    public void Start()
    {
        playerData = new Player();
        userData = new User();

        LoadUserData();
        LoadPlayerData();

        /*---leaderBoard---*/
        leaderboard.Clear();
        playerName.Clear();
        LoadLeaderboardData();
    }

    private void Update()
    {
        dispatcher.PollJobs();
    }

    public TResult RunOnMainThread<TResult>(System.Func<TResult> f)
    {
        return dispatcher.Run(f);
    }

    public void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    private void LoadUserData()
    {
        id = PlayerPrefs.GetString("UserID");

        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error in data retrieval from Users table");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot userNode in snapshot.Children)
                {
                    if (userNode.Key == id)
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


    }

    private void LoadPlayerData()
    {
        id = PlayerPrefs.GetString("UserID");

        FirebaseDatabase.DefaultInstance.GetReference("Player").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error in data retrieval from Player table");
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
                            //Debug.LogFormat("PLAYER ---- Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                            if (kvp.Key == "mastery")
                            {
                                var masteryDict = (IDictionary<string, object>)playerDict["mastery"];
                                foreach (KeyValuePair<string, object> kvp1 in masteryDict)
                                {
                                    //Debug.LogFormat("MASTERY ---- Key = {0}, Value = {1}", kvp1.Key, kvp1.Value);
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

                RunOnMainThread(() =>
                {
                    PlayerPrefs.SetInt("CharacterID", playerData.characterID);

                    // Hide loading screen after finished loading
                    HideLoadingScreen();
                    return 0;
                });
            }
        });
    }



    /*----------------------------------------------------*/
    /*--------------------leaderBoard---------------------*/
    /*----------------------------------------------------*/

    private void LoadLeaderboardData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Player").OrderByChild("totalPoints").LimitToLast(10).ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error in data retrieval from Users table");
            }
            else if (task.IsCompleted)
            {
                // Do something with the data in args.Snapshot
                DataSnapshot snapshot = args.Snapshot;
                DataSnapshot snapshot2 = task.Result;

                foreach (DataSnapshot playerNode in snapshot.Children)
                {
                    //load player data into player object
                    foreach (DataSnapshot userNode in snapshot2.Children)
                    {
                        if (userNode.Key == playerNode.Key)
                        {
                            var playerDict = (IDictionary<string, object>)playerNode.Value;
                            topPlayerData = new Player(playerDict);
                            leaderboard.Insert(0, topPlayerData);
                            
                            var userDict = (IDictionary<string, object>)userNode.Value;
                            topUserData = new User(userDict);
                            playerName.Insert(0, topUserData);
                            break;
                        }
                    }
                }
            }
        });
    }
}
