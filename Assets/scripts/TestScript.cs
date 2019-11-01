using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;

public class TestScript : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseApp app;
    private DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                loadTestData();

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void loadTestData()
    {
        // Insert Questions
        for(int i = 0; i < 10; i++)
        {
            string difficulty;
            if(i > 2) //3,4,5,6
            {
                difficulty = "Hard";
            }
            else if(i > 8) //9,10
            {
                difficulty = "Easy";
            }
            else // 0,1,2
            {
                difficulty = "Medium";
            }


            Question question = new Question(1, 1, 1, difficulty, "Test Question " + i, 1, "Option 1", "Option 2", "Option 3", "Option 4");
            string json = JsonUtility.ToJson(question);
            reference.Child("Questions").Child(i.ToString()).SetRawJsonValueAsync(json);
        }

        // Insert Player
        /*
        Mastery playerMastery = new Mastery(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        Player player = new Player(playerMastery, 1, 0, 0);
        User playerUser = new User(1, "Player Test", "player@e.ntu.edu.sg", "Player");

        string json = JsonUtility.ToJson(playerUser);
        reference.Child("Users").Child(newUser.UserId).SetRawJsonValueAsync(json);

        string json1 = JsonUtility.ToJson(player);
        reference.Child("Player").Child(newUser.UserId).SetRawJsonValueAsync(json1);

        // Insert Teacher
        User teacherUser = new User(2, "Teacher Test", "teacher@ntu.edu.sg", "Teacher");
        */

        // Insert Player
        Register("player@e.ntu.edu.sg", "Test123", "Player");

        // Insert Teacher
        Register("teacher@ntu.edu.sg", "Test123", "Teacher");

        // Insert Challenge
        List<int> challengeQns = new List<int>() ;
        challengeQns.Add(1);
        challengeQns.Add(2);
        challengeQns.Add(3);

        Challenge newChallenge = new Challenge(challengeQns, "teacher");

        string json1 = JsonUtility.ToJson(newChallenge);
        reference.Child("Challenges").Child(newChallenge.challengeId).SetRawJsonValueAsync(json1);


        // Insert Assignment
        //Assignment(string assignmentName, int qnID, int world, int stage, string difficulty, string question, int answer, string o1, string o2, string o3, string o4)
        //Assignment newAssignment = new Assignment("Assignment 1",1,1,1,"Medium",)

        //string json2 = JsonUtility.ToJson(newAssignment);
        //reference.Child("Challenges").Child("1").SetRawJsonValueAsync(json1);
    }

    public void Register(string email, string password, string userType)
    {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);

                return;
            }

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            string name = email.Split('@')[0];
            Debug.LogFormat("--- NAME : ", name);

            User user = new User();

            if (userType == "Player")
            {
                Mastery playerMastery = new Mastery(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                Player player = new Player(playerMastery, 1, 0, 0);
                user = new User(newUser.UserId, name, email, userType);

                string json = JsonUtility.ToJson(user);
                reference.Child("Users").Child(newUser.UserId).SetRawJsonValueAsync(json);

                string json1 = JsonUtility.ToJson(player);
                reference.Child("Player").Child(newUser.UserId).SetRawJsonValueAsync(json1);
            }
            else
            {
                user = new User(newUser.UserId, name, email, userType);
                string json = JsonUtility.ToJson(user);

                reference.Child("Users").Child(newUser.UserId).SetRawJsonValueAsync(json);
            }

        });
    }
}
