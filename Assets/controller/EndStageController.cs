using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndStageController : MonoBehaviour
{
    public Text scoreTxt, stageTxt;
    public Text feedbackTxt;

    // Start is called before the first frame update
    void Start()
    {
        scoreTxt.text = PlayerPrefs.GetInt("Score").ToString();
        stageTxt.text = "Stage " + PlayerPrefs.GetInt("SelectedStage").ToString();
        feedbackTxt.text = "Right ans/ Qns attempted: " + PlayerPrefs.GetInt("stageCorrectAns") + "/" + PlayerPrefs.GetInt("stageQnsAttempt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OkClick()
    {
        SceneManager.LoadScene("Stage");
    }
    
    public void RestartClick()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitToMenuClick()
    {
        SceneManager.LoadScene("PlayerMain");
    }
}
