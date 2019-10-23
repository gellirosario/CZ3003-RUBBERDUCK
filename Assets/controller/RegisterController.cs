using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;



public class RegisterController : MonoBehaviour
{
    private FirebaseAuth auth;
    public InputField emailInput, passwordInput, confirmInput;
    public Button registerBtn;
    public Text messageTxt;

    private bool errorFound = false;
    private bool success = false;
    private string message;
    public string userType;
    
    private FirebaseApp app;
    private DatabaseReference reference;
    
    // Regex ntu email pattern
    public const string MatchStudentEmailPattern = @"[a-zA-Z0-9]{0,}([.]?[a-zA-Z0-9]{1,})[@](e.ntu.edu.sg)";
    public const string MatchTeacherEmailPattern = @"[a-zA-Z0-9]{0,}([.]?[a-zA-Z0-9]{1,})[@](ntu.edu.sg)";
    
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
		messageTxt.text = "";
        message = "";
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://teamrubberduck-1420e.firebaseio.com/");
                reference = FirebaseDatabase.DefaultInstance.RootReference;

            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        
        registerBtn.onClick.AddListener(() => Register(emailInput.text, passwordInput.text, confirmInput.text));
    }

    private void Update()
    {
        if (errorFound)
            UpdateMessage();

        if (success)
        {
            ClearDetails();
            message = "Successfully Registered!";
            UpdateMessage();
            success = false;
        }
            
    }

    private void UpdateMessage()
    {
        messageTxt.text = message;
        errorFound = false;
        Invoke("ClearMessage", 3);
    }

    void ClearMessage()
    {
        messageTxt.text = "";
    }

    public void Register(string email, string password, string confirm)
    {

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
        {
            //Error handling
			messageTxt.text = "Please enter all details";
            return;
        }

        if (password != confirm)
        {
            //Error handling
            messageTxt.text = "Please make sure Password and Confirm password is the same";
            return;
        }

        
        if(Regex.IsMatch(email, MatchStudentEmailPattern))
        {
            userType = "Player";
        }
        else if(Regex.IsMatch(email, MatchTeacherEmailPattern))
        {
            userType = "Teacher";
        }
        else
        {
            //Error handling
            messageTxt.text = "Please use your NTU email";
            return;
        }
        
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

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
            
            string name = email.Split('@')[0];
            Debug.LogFormat("--- NAME : ",name);
            
            // TODO: ADD TO REALTIME DATABASE???
            User user = new User();

            if (userType == "Player")
            {
                Mastery playerMastery = new Mastery(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
                Player player = new Player(playerMastery, 1,0, 0); 
                user = new User(newUser.UserId, name, email, userType, player);
                
                string json = JsonUtility.ToJson(user);
                reference.Child("Users").Child(newUser.UserId).SetRawJsonValueAsync(json);
                
                //string json1 = JsonUtility.ToJson(player);
                //reference.Child("Player").Child(newUser.UserId).SetRawJsonValueAsync(json1);
            }
            else
            {
                user = new User(newUser.UserId, name, email, userType);
                string json = JsonUtility.ToJson(user);
                reference.Child("Users").Child(newUser.UserId).SetRawJsonValueAsync(json);
            }
            
            
            // Success Message
            success = true;
            
        });
    }
	
	public void ClearDetails()
	{
		emailInput.text = "";
		passwordInput.text = "";
        confirmInput.text = "";
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
            default:
                msg = "Error occured";
                break;
        }
        return msg;
    }
}
