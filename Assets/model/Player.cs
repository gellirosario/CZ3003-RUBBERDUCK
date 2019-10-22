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
    public Player(IDictionary <string, object> dict)
    {
        //pls refer to question model
    }
}

