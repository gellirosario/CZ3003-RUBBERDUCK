using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareCode : MonoBehaviour
{
    public void ClickShare()
    {
        StartCoroutine(TakeSSAndShare()); 
    }
    private IEnumerator TakeSSAndShare()
    {
        yield return new WaitForEndOfFrame();

        new NativeShare().SetSubject("").SetText("Join my challenge! Code: " + PlayerPrefs.GetString("NewChallengeID")).Share();
    }
}
