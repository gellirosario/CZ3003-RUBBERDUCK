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
    public Button loginButton;
    public Text errorMessage;

    private void Awake()
    {
        dispatcher = new ThreadDispatcher();
        errorMessage.text = "";
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void UpdateErrorMessage(string message)
    {
        errorMessage.text = message;
        Invoke("ClearErrorMessage", 3);
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
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }
           
            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

            
            RunOnMainThread(() =>
            {
                PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                errorMessage.text = "Logged In!";
                SceneManager.LoadScene("Main");
                return 0;
            });
           

        });
    }

    void Update()
    {
        dispatcher.PollJobs();  // 未処理のジョブがあれば実行
    }

    public TResult RunOnMainThread<TResult>(System.Func<TResult> f)
    {
        return dispatcher.Run(f);
    }

}

