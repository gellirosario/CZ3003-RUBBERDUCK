using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LogInController : MonoBehaviour
{
    private FirebaseAuth auth;
    Firebase.Auth.FirebaseUser currentuser;

    private ThreadDispatcher dispatcher;

    public InputField emailInput, passwordInput;
    public Button loginBtn;
    public Text messageTxt;

    private bool errorFound = false;
    private string message;

    public const string MatchTeacherEmailPattern = @"[a-zA-Z0-9]{0,}([.]?[a-zA-Z0-9]{1,})[@](ntu.edu.sg)";
    
    private void Awake()
    {
        dispatcher = new ThreadDispatcher();
        message = "";
        messageTxt.text = "";
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        

        //------logOut-----//
        onDestroy();
        
    }

    private void Update()
    {
        dispatcher.PollJobs();

        if (errorFound)
            UpdateMessage();
    }

    private void UpdateMessage()
    {
        errorFound = false;
        messageTxt.text = message;
        Invoke("ClearMessage", 3);
    }

    public void LogIn()
    {
        if (string.IsNullOrEmpty(emailInput.text.Trim()) || string.IsNullOrEmpty(passwordInput.text.Trim()))
        {
            //Error handling
            messageTxt.text = "Please enter all details";
            return;
        }
        
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
                
                foreach (var exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = string.Format("AuthError.{0}: ",
                            ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    Debug.Log("number- "+ authErrorCode +"the exception is- "+ exception.ToString());
                    string code = ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString();
                    Debug.Log(code);

                    message = GetErrorMessage((Firebase.Auth.AuthError)firebaseEx.ErrorCode);
                    Debug.Log("---- Message " + message);
                    errorFound = true;
                }
                    
                return;
            }
           
            FirebaseUser user = task.Result;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
            Debug.LogFormat("User signed in successfully: ({0})", user.UserId);

            
            RunOnMainThread(() =>
            {
                //PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                PlayerPrefs.SetString("UserID", user.UserId.ToString());
                Debug.LogFormat("---USER ID SET---" + user.UserId.ToString());

                 if(Regex.IsMatch(user.Email, MatchTeacherEmailPattern))
                {
                    SceneManager.LoadScene("TeacherMain");
                }
                else
                {
                    SceneManager.LoadScene("PlayerMain");
                }

                
                return 0;
            });
           

        });
    }

    public TResult RunOnMainThread<TResult>(System.Func<TResult> f)
    {
        return dispatcher.Run(f);
    }

    private void ClearMessage()
    {
        messageTxt.text = "";
    }
    
    private string GetErrorMessage(AuthError errorCode)
    {
        var msg = "";
        
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                msg = "Wrong password";
                break;
            case AuthError.MissingPassword:
                msg = "Please input a password";
                break;
            case AuthError.WeakPassword:
                msg = "Weak Password";
                break;
            case AuthError.WrongPassword:
                msg = "Please input a correct password";
                break;
            case AuthError.EmailAlreadyInUse:
                msg = "Email already in use";
                break;
            case AuthError.InvalidEmail:
                msg = "Email is invalid";
                break;
            case AuthError.MissingEmail:
                msg = "Email does not exist";
                break;
			case AuthError.UserNotFound:
                msg = "User not found";
                break;
            default:
                msg = "Error occured";
                break;
        }
        return msg;
    }


    //-------logOut--------//
    private void onDestroy()
    {
        Destroy(GameObject.Find("ProfileLoader"));
        Destroy(GameObject.Find("QuestionLoader"));
        PlayerPrefs.DeleteAll();
        auth.SignOut();
        //auth = null;

        auth.StateChanged -= AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != currentuser)
        {
            bool signedIn = currentuser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && currentuser != null)
            {
                Debug.Log("Signed out " + currentuser.UserId);
            }
            currentuser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + currentuser.UserId);
            }
        }
    }

}

