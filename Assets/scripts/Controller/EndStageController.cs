using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndStageController : MonoBehaviour
{
    public Text scoreTxt, stageTxt;
    public Text feedbackTxt;
    public GameObject star1, star2, star3;

    // Start is called before the first frame update
    void Start()
    {
        SetStars();
        scoreTxt.text = PlayerPrefs.GetInt("Score").ToString();
        stageTxt.text = "Stage " + PlayerPrefs.GetInt("SelectedStage").ToString();
        feedbackTxt.text = "Right ans/ Qns attempted: " + PlayerPrefs.GetInt("stageCorrectAns") + "/" + PlayerPrefs.GetInt("stageQnsAttempt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStars()
    {
        int stars = PlayerPrefs.GetInt("Stars");

        switch (stars)
        {
            case 3:
                break;
            case 2:
                star3.SetActive(false);
                break;
            case 1:
                star3.SetActive(false);
                star2.SetActive(false);
                break;
            case 0:
                star3.SetActive(false);
                star2.SetActive(false);
                star1.SetActive(false);
                break;
            default:
                break;
        }

    }

    public void OkClick()
    {
        SceneManager.LoadScene("PlayerMain");
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
