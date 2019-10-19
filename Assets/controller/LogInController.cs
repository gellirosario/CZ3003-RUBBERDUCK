using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class LogInController : MonoBehaviour
{
    // Start is called before the first frame update    
    private FirebaseAuth auth;


    public InputField EmailInputField, PasswordInputField;
    public Button LoginButton;


    public Text ErrorText;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        //Just an example to save typing in the login form
       

        
        LoginButton.onClick.AddListener(() => Login(EmailInputField.text.Trim(), PasswordInputField.text.Trim()));
    }



    private void UpdateErrorMessage(string message)
    {
        ErrorText.text = message;
        Invoke("ClearErrorMessage", 3);
    }




    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
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
             Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
            SceneManager.LoadScene("play");

        });
    }




}

