using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    public string challengeId;
    public string challengeCreator;
    public List<int> challengeQns; //List of qn ids in the challenge

    public Challenge(List<int> challengeQns)
    {
        this.challengeQns = challengeQns;
    }

    public Challenge(List<int> qns, string creator)
    {
        this.challengeId = generateId();
        this.challengeQns = qns;
        this.challengeCreator = creator;
    }
    public Challenge(string id, List<int> qns, string creator)
    {
        this.challengeId = id;
        this.challengeQns = qns;
        this.challengeCreator = creator;
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
