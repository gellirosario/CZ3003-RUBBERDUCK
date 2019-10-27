using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    // Player Details
    public Mastery mastery;
    public int characterID;
    public int totalPoints;
    public int totalQnAnswered;
    
    public Player(){}

    public Player(Mastery mastery, int characterID, int totalPoints, int totalQnAnswered)
    {
        this.mastery = mastery;
        this.characterID = characterID;
        this.totalPoints = totalPoints;
        this.totalQnAnswered = totalQnAnswered;
    }

    // Updating IDictionary to Class
    public Player(IDictionary<string, object> dict)
    {
        this.totalPoints = int.Parse(dict["totalPoints"].ToString());
        this.totalQnAnswered = int.Parse(dict["totalQnAnswered"].ToString());

    }

    // Updating IDictionary to Class
    public Player(IDictionary <string, object> dict, IDictionary<string, object> mdict)
    {
        this.characterID = int.Parse(dict["characterID"].ToString());
        this.totalPoints = int.Parse(dict["totalPoints"].ToString());
        this.totalQnAnswered = int.Parse(dict["totalQnAnswered"].ToString());
        this.mastery = new Mastery(mdict);
        
    }
}

