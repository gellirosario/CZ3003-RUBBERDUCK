using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndStageController : MonoBehaviour
{
    public Text scoreTxt, stageTxt;

    // Start is called before the first frame update
    void Start()
    {
        scoreTxt.text = PlayerPrefs.GetInt("Score").ToString();
        stageTxt.text = "Stage " + PlayerPrefs.GetInt("SelectedStage").ToString();
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
}
