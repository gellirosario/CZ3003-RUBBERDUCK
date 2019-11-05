using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Extensions;


public class ChallengeLeaderboardController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;

    public Text positions, playerNames, scores;

    private List<ChallengeEntry> challengeEntries;

    void Awake()
    {
        Debug.Log("Initiating Challenge LB Controller...");
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

    void OnEnable()
    {
        positions.text = "";
        playerNames.text = "";
        scores.text = "";

        retrieveChallengeFromDB();
    }

    private void retrieveChallengeFromDB()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Challenges").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error retrieving from challenges table.");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot challenge in snapshot.Children)
                {
                    if (challenge.Key == PlayerPrefs.GetString("challengeID"))
                    {
                        Debug.Log("THIS CHALLENGE IS : " + PlayerPrefs.GetString("challengeID"));
                        string challengeData = challenge.GetRawJsonValue();
                        challengeEntries = JsonUtility.FromJson<Challenge>(challengeData).challengePlayers;
                        Debug.Log("Found challenge entries: " + challengeEntries.Count);
                        PrintChallengeEntries(challengeEntries);
                    }
                }
            }
        });
    }

    void PrintChallengeEntries(List<ChallengeEntry> entriesToPrint)
    {
        //sort by descending score
        entriesToPrint.Sort(delegate (ChallengeEntry x, ChallengeEntry y)
        {
            return y.score.CompareTo(x.score);
        });

        for (int i = 0; i < entriesToPrint.Count; i++){
            positions.text += (i + 1) + ".\n";
            playerNames.text += entriesToPrint[i].name + "\n";
            scores.text += entriesToPrint[i].score + "\n";
        }
    }

    public void CloseLeaderboard()
    {
        gameObject.SetActive(false);
    }

}
