using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //public List<Question> questionList = new List<>();
    private Question[] questions = null;
    public Question[] Questions { get { return questions; } }

    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private int score;

}
