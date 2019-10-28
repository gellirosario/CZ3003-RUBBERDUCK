using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateChallengeController : MonoBehaviour
{
    public Text noOfQns;
    public Dropdown difficultyDropdown;

    public const int MIN_QNS = 1;
    public const int MAX_QNS = 10;

    public string difficulty;
    public int world;
    public int stage;

    public void IncrementNoOfQns()
    {
        int val = int.Parse(noOfQns.text);
        if (val < MAX_QNS)
            val++;
        noOfQns.text = val.ToString();
    }

    public void DecrementNoOfQns()
    {
        int val = int.Parse(noOfQns.text);
        if (val > MIN_QNS)
            val--;
        noOfQns.text = val.ToString();
    }

    public void CreateChallenge()
    {
        string difficultySelected = difficultyDropdown.options[difficultyDropdown.value].text;

        //some test code for now
        List<Question> questionList = RandomizeQuestions(int.Parse(noOfQns.text), 1, 1, difficultySelected);
    }
    //generates a randomized list of questions with specified difficulty, world, stage, and number of questions
    private List<Question> RandomizeQuestions(int NoOfQns, int world, int stage, string difficulty)
    {
        List<Question> questionList = new List<Question>();

        //get filtered list from QuestionLoader
        List<Question> filteredList = QuestionLoader.Instance.FilterQuestionsByWorldAndStage(world, stage);
        Debug.Log("Filtered list by world " + world + ", stage " + stage);

        //filter further by difficulty
        filteredList = QuestionLoader.Instance.FilterQuestionsListByDifficulty(filteredList, difficulty);
        Debug.Log("Filtered list by difficulty: " + difficulty);
        Debug.Log("Filtered list has " + filteredList.Count + " items.");

        //shuffle list
        questionList = ShuffleList(filteredList, NoOfQns);

        return questionList;
    }


    //function to shuffle question list
    private List<Question> ShuffleList(List<Question> questionList, int NoOfQns)
    {
        List<Question> randomList = new List<Question>();

        int randomIndex = 0;

        while(questionList.Count > 0 && NoOfQns > 0)
        {
            randomIndex = Random.Range(0, questionList.Count);
            randomList.Add(questionList[randomIndex]);
            Debug.Log("Added index " + randomIndex + " to new list");
            Debug.Log("New list now has " + randomList.Count + " items");
            //remove from original list to avoid duplicates
            questionList.RemoveAt(randomIndex);
            NoOfQns--;
        }
        return randomList;
    }
}
