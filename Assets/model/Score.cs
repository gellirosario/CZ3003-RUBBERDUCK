using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public int score;

    public Score()
    {
        score = 0;
    }

    public void AddScoreAccordingToDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                score = score + 10;
                break;
            case "Normal":
                score = score + 20;
                break;
            case "Hard":
                score = score + 40;
                break;
        }
    }

    public void ResetScore()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }
}
