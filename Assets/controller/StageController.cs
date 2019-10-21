using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public void LoadNextSceneLevel(int selectedStage)
    {
        if(selectedStage == null)
            return;
        
        PlayerPrefs.SetInt("SelectedStage", selectedStage);

        Debug.Log("Preferences set: Selected Stage - " + selectedStage.ToString());
        if (PlayerPrefs.GetInt("SelectedStage") != null)
        {
            SceneManager.LoadScene("Level");
        }
        else
        {
            SceneManager.LoadScene("Stage");
        }
        
    }
}
