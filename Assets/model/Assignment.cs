using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssignmentEntry //this class is for storing player ids that have completed the assignment and their score, since unity does not support serializing dictionaries to json
{
    public string id; //player id
    public int score;

    public AssignmentEntry(string id, int score)
    {
        this.id = id;
        this.score = score;
    }
}

public class Assignment 
{
    public string assignmentId;
    public string assignmentCreator;
    public List<int> assignmentQns; //List of qn ids in the assignment
    public List<AssignmentEntry> assignmentPlayers;

    public string assignmentName;
    public int qnID;
    public int world;
    public int stage;
    public string difficulty;
    public string question;
    public int answer;
    public string option1;
    public string option2;
    public string option3;
    public string option4;



    public Assignment(List<int> assignmentQns)
    {
        this.assignmentQns = assignmentQns;
    }

    public Assignment(List<int> qns, string creator)
    {
        this.assignmentId = generateId();
        this.assignmentQns = qns;
        this.assignmentCreator = creator;
        this.assignmentPlayers = new List<AssignmentEntry>();
    }

    /*public Assignment(string id, List<int> qns, string creator, List<AssignmentEntry> players)
    {
        this.assignmentId = id;
        this.assignmentQns = qns;
        this.assignmentCreator = creator;
        this.assignmentPlayers = players;
    }*/

    public Assignment(string id, List<int> qns, string creator)
    {
        this.assignmentId = id;
        this.assignmentQns = qns;
        this.assignmentCreator = creator;
    }

    public void addPlayerAndScore(string id, int score) //use this function to when a player has completed a challenge
    {
        this.assignmentPlayers.Add(new AssignmentEntry(id, score));
    }

    //generate an 5-character long random ID for the assignments
    private string generateId()
    {
        string id = "";

        string alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < 5; i++)
        {
            id += alphanumeric[Random.Range(0, alphanumeric.Length)];
        }

        return id;
    }




/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Assignment()
    {
    }

    public Assignment(string assignmentName, int qnID, int world, int stage, string difficulty, string question, int answer, string o1, string o2, string o3, string o4)
    {
       
        this.assignmentName = assignmentName;
        this.qnID = qnID;
        this.world = world;
        this.stage = stage;
        this.difficulty = difficulty;
        this.question = question;
        this.answer = answer;
        this.option1 = o1;
        this.option2 = o2;
        this.option3 = o3;
        this.option4 = o4;
    }

    // Updating IDictionary to Class
    public Assignment(IDictionary<string, object> dict)
    {
        
        this.assignmentName = dict["assignmentName"].ToString();
        this.qnID = int.Parse(dict["qnID"].ToString());
        this.world = int.Parse(dict["world"].ToString());
        this.stage = int.Parse(dict["stage"].ToString());
        this.difficulty = dict["difficulty"].ToString();
        this.question = dict["question"].ToString();
        this.answer = int.Parse(dict["answer"].ToString());
        this.option1 = dict["option1"].ToString();
        this.option2 = dict["option2"].ToString();
        this.option3 = dict["option3"].ToString();
        this.option4 = dict["option4"].ToString();
    }

    
}
