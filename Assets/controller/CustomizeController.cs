using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class CustomizeController : MonoBehaviour
{
    private FirebaseApp app;
    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;
    //private string userId;
    public Text messageTxt;
    private int selectIndex;
    [Header("UI reference")]
    [SerializeField] private Image characterSplash;
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();

    private void UpdateCharacterSelectionUI()
    {
        characterSplash.sprite = characterList[selectIndex].splash;
        //haracterSplash.sprite = characterList[selectIndex].splash;
        Debug.Log(string.Format("uodate"));
    }
    [System.Serializable]
    public class CharacterSelectObject
    {
        public string characterName;
        public Sprite splash;
    }

    public void RightArrow()
    {
        selectIndex++;
        if (selectIndex == characterList.Count)
            selectIndex = 0;
        //Debug.Log(string.Format("right"));
        UpdateCharacterSelectionUI();
    }
    public void LeftArrow()
    {
        selectIndex--;
        if (selectIndex <0)
            selectIndex = characterList.Count -1;
        //Debug.Log(string.Format("left"));
        UpdateCharacterSelectionUI();
    }
    public void Confirm()
    {
        Debug.Log(string.Format("selected {0}:{1} ",selectIndex,characterList[selectIndex].characterName));
        reference.Child("Player").Child(PlayerPrefs.GetString("UserID")).Child("characterID").SetValueAsync(selectIndex);
        messageTxt.text = "Character change to "+characterList[selectIndex].characterName;
    }
    // Start is called before the first frame update
    private void Start()
    {
        messageTxt.text = "";
        UpdateCharacterSelectionUI();
        Debug.Log(string.Format("start"));
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
        /*Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            //string name = user.DisplayName;
            //string email = user.Email;
            //System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            userId = user.UserId;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
