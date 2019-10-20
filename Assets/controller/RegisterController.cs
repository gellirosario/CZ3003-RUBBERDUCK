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
    public InputField emailInput, passwordInput, confirmInput;
    public Button registerBtn;
    public Text messageTxt;



    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
		messageTxt.text = "";
        registerBtn.onClick.AddListener(() => Register(emailInput.text, passwordInput.text, confirmInput.text));
    }


    private void UpdateMessage(string message)
    {
        messageTxt.text = message;
        Invoke("ClearMessage", 3);
    }

    void ClearErrorMessage()
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
                    UpdateMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
			ClearDetails();
            UpdateMessage("Signup Success");

            //SceneManager.LoadScene("Login");
        });
    }
	
	public void ClearDetails()
	{
		emailInput.text = "";
		passwordInput.text = "";
	}
}
