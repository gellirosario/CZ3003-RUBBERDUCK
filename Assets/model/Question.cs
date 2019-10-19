[System.Serializable]
public class Question
{
    public enum Difficulty { Easy, Medium, Hard }

    public int world;
    public int stage;
    public Difficulty difficulty;
    public string question;
    public int answer;
    public string option1;
    public string option2;
    public string option3;
    public string option4;
}
