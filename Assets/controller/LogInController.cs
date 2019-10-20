using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LogInController : MonoBehaviour
{
    private FirebaseAuth auth;
    private ThreadDispatcher dispatcher;

    public InputField emailInput, passwordInput;
    public Button loginBtn;
    public Text messageTxt;

    private void Awake()
    {
        dispatcher = new ThreadDispatcher();
        messageTxt.text = "";
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void UpdateMessage(string message)
    {
        messageTxt.text = message;
        Invoke("ClearMessage", 3);
    }

    public void LogIn()
    {
        auth.SignInWithEmailAndPasswordAsync(emailInput.text.Trim(), passwordInput.text.Trim()).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }
           
            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

            
            RunOnMainThread(() =>
            {
                PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                SceneManager.LoadScene("Main");
                return 0;
            });
           

        });
    }

    void Update()
    {
        dispatcher.PollJobs();  
    }

    public TResult RunOnMainThread<TResult>(System.Func<TResult> f)
    {
        return dispatcher.Run(f);
    }

}

