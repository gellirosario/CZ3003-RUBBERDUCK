using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    public string challengeId;
    public List<int> challengeQns; //List of qn ids in the challenge

    public Challenge(List<int> qns)
    {
        this.challengeId = generateId();
        this.challengeQns = qns;
    }
    public Challenge(string id, List<int> qns)
    {
        this.challengeId = id;
        this.challengeQns = qns;
    }

    //generate an 8-character long random ID for the challenge
    private string generateId()
    {
        string id = "";

        string alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        for(int i = 0; i < 8; i++)
        {
            id += alphanumeric[Random.Range(0, alphanumeric.Length)];
        }

        return id;
    }
}
