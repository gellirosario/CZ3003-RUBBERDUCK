using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndChallengeController : MonoBehaviour
{
    public Text scoreText, resultText;
    public GameObject leaderboard;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("Score").ToString();
        resultText.text = "Result: " + PlayerPrefs.GetInt("stageCorrectAns") + "/" + PlayerPrefs.GetInt("stageQnsAttempt");
    }

    public void ToggleLeaderboard()
    {
        if (leaderboard != null)
        {
            bool isActive = leaderboard.activeSelf;
            leaderboard.SetActive(!isActive);
        }
    }

    public void OkClick()
    {
        SceneManager.LoadScene("PlayerMain");
    }
}
