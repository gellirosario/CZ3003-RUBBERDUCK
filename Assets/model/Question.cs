[System.Serializable]
public class Question
{
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

    public Question(int world, int stage, string difficulty, string question, int answer, string o1, string o2, string o3, string o4) {
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
    /*
    public Dictionary<string, Object> ToDictionary() {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        
        result["world"] = world;
        result["stage"] = stage;
        result["difficulty"] = difficulty;
        result["question"] = question;
        result["answer"] = answer;
        result["option1"] = option1;
        result["option2"] = option2;
        result["option3"] = option3;
        result["option4"] = option4;
        
        return result;
    }*/
}
