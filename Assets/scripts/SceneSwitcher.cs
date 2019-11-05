using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName);
    }

    public void GoToPlayerOrTeacherMain()
    {
        if(PlayerPrefs.GetString("UserType") == "Player")
        {
            SceneManager.LoadScene("PlayerMain");
        }
        else if (PlayerPrefs.GetString("UserType") == "Teacher")
        {
            SceneManager.LoadScene("TeacherMain");
        }
    }

}
