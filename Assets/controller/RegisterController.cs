using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class RegisterController : MonoBehaviour
{
    private FirebaseAuth auth;
    public InputField UserNameInput, PasswordInput;
    public Button RegisterButton;
    public Text ErrorText;



    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        

        RegisterButton.onClick.AddListener(() => Signup(UserNameInput.text, PasswordInput.text));
    }


    private void UpdateErrorMessage(string message)
    {
        ErrorText.text = message;
        Invoke("ClearErrorMessage", 3);
    }

    void ClearErrorMessage()
    {
        ErrorText.text = "";
    }

    public void Signup(string email, string password)
    {

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            //Error handling
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
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UpdateErrorMessage("Signup Success");

            SceneManager.LoadScene("play");
        });
    }


    // Update is called once per frame
    
}
