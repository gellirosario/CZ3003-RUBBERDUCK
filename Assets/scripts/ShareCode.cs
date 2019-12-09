using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareCode : MonoBehaviour
{
    public void ClickShare(string challengeOrAssignment)
    {
        StartCoroutine(TakeSSAndShare(challengeOrAssignment)); 
    }
    private IEnumerator TakeSSAndShare(string challengeOrAssignment)
    {
        yield return new WaitForEndOfFrame();

        if(challengeOrAssignment == "challenge")
            new NativeShare().SetSubject("").SetText("Join my challenge! Code: " + PlayerPrefs.GetString("NewChallengeID")).Share();
        else
            new NativeShare().SetSubject("").SetText("Join my assignment! Code: " + PlayerPrefs.GetString("NewAssignmentID")).Share();
    }
}
