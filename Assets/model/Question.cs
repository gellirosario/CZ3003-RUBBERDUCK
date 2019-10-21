using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Question
{
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
    
    public Question() {
    }

    public Question(int qnID, int world, int stage, string difficulty, string question, int answer, string o1, string o2, string o3, string o4)
    {
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

    public Question(IDictionary <string, object> dict)
    {
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
