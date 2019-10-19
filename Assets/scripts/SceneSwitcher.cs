using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
