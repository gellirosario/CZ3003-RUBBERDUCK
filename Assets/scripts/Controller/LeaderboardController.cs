using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
   
    public Text qns, pts, topName, num, nullMsg;
    
    void Start()
    {
        print();
    }

    void print()
    { 
   
        int i = 1;
        foreach (var item in ProfileLoader.leaderboard)
        {
            pts.text += item.totalPoints + "\n";
            qns.text += item.totalQnAnswered + "\n";
        }

        foreach (var item in ProfileLoader.playerName)
        {
            topName.text += item.name + "\n";
            num.text += i + ".\n";
            i++;
        }

        if(i==1)
        {
            nullMsg.text = "No data found.";
        }

    }

}
