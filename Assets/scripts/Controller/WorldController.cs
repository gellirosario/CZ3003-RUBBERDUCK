using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    public void LoadNextSceneStage(int selectedWorld)
    {
        if(selectedWorld == null)
            return;
        
        PlayerPrefs.SetInt("SelectedWorld", selectedWorld);

        Debug.Log("Preferences set: Selected World - " + selectedWorld.ToString());
        if (PlayerPrefs.GetInt("SelectedWorld") != null)
        {
            SceneManager.LoadScene("Stage");
        }
        else
        {
            SceneManager.LoadScene("World");
        }
        
    }
}
