using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ChallengeEntry //this class is for storing player ids that have completed the challenge and their score, since unity does not support serializing dictionaries to json
{
    public string id; //player id
    public int score;

    public ChallengeEntry(string id, int score)
    {
        this.id = id;
        this.score = score;
    }
}
public class Challenge
{
    public string challengeId;
    public string challengeCreator;
    public List<int> challengeQns; //List of qn ids in the challenge
    public List<ChallengeEntry> challengePlayers;

    public Challenge(List<int> challengeQns)
    {
        this.challengeQns = challengeQns;
    }

    public Challenge(List<int> qns, string creator)
    {
        this.challengeId = generateId();
        this.challengeQns = qns;
        this.challengeCreator = creator;
        this.challengePlayers = new List<ChallengeEntry>();
    }

    public Challenge(string id, List<int> qns, string creator, List<ChallengeEntry> players)
    {
        this.challengeId = id;
        this.challengeQns = qns;
        this.challengeCreator = creator;
        this.challengePlayers = players;
    }

    public void addPlayerAndScore(string id, int score) //use this function to when a player has completed a challenge
    {
        this.challengePlayers.Add(new ChallengeEntry(id, score));
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
