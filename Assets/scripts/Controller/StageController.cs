using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public void Start(){

        var s1 = "";
        var s2 = "";
        var s3 = "";
        int selectedWorld = PlayerPrefs.GetInt("SelectedWorld");

        if(selectedWorld != null){
            switch (selectedWorld)
            {
                case 1:
                    s1 = "1 | Software engineering principles";
                    s2 = "2 | Requirement Analysis";
                    s3 = "3 | Modelling";
                    break;
                case 2:
                    s1 = "1 | Architectural Design";
                    s2 = "2 | Design Concepts";
                    s3 = "3 | Component Level Designs";
                    break;
                case 3:
                    s1 = "1 | Software elements";
                    s2 = "2 | Software components";
                    s3 = "3 | Software configuration";
                    break;
                case 4:
                    s1 = "1 | Software Testing Techniques and Strategies ";
                    s2 = "2 | Testing Application";
                    s3 = "3 | Software Testing";
                    break;
                case 5:
                    s1 = "1 | Software Management";
                    s2 = "2 | Software Configuration";
                    s3 = "3 | Quality Management";
                    break;
                default:
                    s1 = "Stage 1";
                    s2 = "Stage 2";
                    s3 = "Stage 3";
                    break;
            }
        }

        GameObject.Find("Stage1Button").GetComponentInChildren<Text>().text = s1;
        GameObject.Find("Stage2Button").GetComponentInChildren<Text>().text = s2;
        GameObject.Find("Stage3Button").GetComponentInChildren<Text>().text = s3;

    }

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
