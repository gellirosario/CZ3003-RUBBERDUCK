using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionItem1
{
    public string assignmentTxt;
    public string worldTxt;
    public string stageTxt;
    public string difficultyTxt;
    public string questionIDTxt;
    public string questionTxt;
    public string o1Txt;
    public string o2Txt;
    public string o3Txt;
    public string o4Txt;
    public string answerTxt;
}

public class QuestionScrollList : MonoBehaviour
{
    public List<QuestionItem1> questionItemList;
    public List<Question> questionList = new List<Question>();
    public Transform contentPanel;
    public SimpleObjectPool questionObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        questionList = retrieveQuestionDataFromQuestionController();
        RefreshDisplay();
    }

    private List<Question> retrieveQuestionDataFromQuestionController()
    {
        List<Question> qnList = new List<Question>(PlayerPrefs.GetInt("QuestionListCount"));
     
        if (qnList.Capacity == 0)
            return null;

        int questionListCount = qnList.Capacity;

        print("In questionscrolllist count: " + questionListCount);

        for (int i = 0; i < questionListCount; i++)
        {
            Question question = new Question();
            qnList.Add(question);
            qnList[i].qnID = PlayerPrefs.GetInt("QuestionList(qnID)" + i);
            qnList[i].world = PlayerPrefs.GetInt("QuestionList(world)" + i);
            qnList[i].stage = PlayerPrefs.GetInt("QuestionList(stage)" + i);
            qnList[i].difficulty = PlayerPrefs.GetString("QuestionList(difficulty)" + i);
            qnList[i].question = PlayerPrefs.GetString("QuestionList(question)" + i);
            qnList[i].answer = PlayerPrefs.GetInt("QuestionList(answer)" + i);
            qnList[i].option1 = PlayerPrefs.GetString("QuestionList(option1)" + i);
            qnList[i].option2 = PlayerPrefs.GetString("QuestionList(option2)" + i);
            qnList[i].option3 = PlayerPrefs.GetString("QuestionList(option3)" + i);
            qnList[i].option4 = PlayerPrefs.GetString("QuestionList(option4)" + i);
        }

        for (int i = 0; i < qnList.Count; i++)
        {
            print(i);
            print("Qn id:" + qnList[i].qnID + "world: " + qnList[i].world + qnList[i].stage + qnList[i].difficulty + qnList[i].question +
                    qnList[i].answer + qnList[i].option1 + qnList[i].option2 + qnList[i].option3 + qnList[i].option4);
        }

        return qnList;
    }

    void RefreshDisplay()
    {
        RemoveQuestionContainers();
        AddQuestionContainers();
    }

    private void RemoveQuestionContainers()
    {
        questionItemList.Clear();
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            questionObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddQuestionContainers()
    {
        print("Global QuestionList with: " + questionList.Capacity + " items");
        if (questionList.Capacity == 0)
            return;
        questionItemList.Capacity = questionList.Capacity;
        print("Item List capacity count: " + questionItemList.Capacity);
        int itemQuestionCount = questionItemList.Capacity;

        for (int i = 0; i < itemQuestionCount; i++)
        {
            if (questionList[i].world == 1 && questionList[i].stage == 1)
            {
                QuestionItem1 questionItem = new QuestionItem1();
                questionItemList.Add(questionItem);

                int topic = questionList[i].world;
                int subtopic = questionList[i].stage;

                switch (topic)
                {
                    case 1:
                        questionItem.worldTxt = "Topic: Requirements";
                        switch (subtopic)
                        {
                            case 1:
                                questionItem.stageTxt = "Subtopic: Software Engineering Principles";
                                break;
                            case 2:
                                questionItem.stageTxt = "Subtopic: Requirements Analysis";
                                break;
                            case 3:
                                questionItem.stageTxt = "Subtopic: Modelling";
                                break;
                        }
                        break;

                    case 2:
                        questionItem.worldTxt = "Topic: Design";
                        switch (subtopic)
                        {
                            case 1:
                                questionItem.stageTxt = "Subtopic: Architectural Designs";
                                break;
                            case 2:
                                questionItem.stageTxt = "Subtopic: Design Concepts";
                                break;
                            case 3:
                                questionItem.stageTxt = "Subtopic: Component Level Designs";
                                break;
                        }
                        break;

                    case 3:
                        questionItem.worldTxt = "Topic: Testing";
                        switch (subtopic)
                        {
                            case 1:
                                questionItem.stageTxt = "Subtopic: Software Elements";
                                break;
                            case 2:
                                questionItem.stageTxt = "Subtopic: Software Components";
                                break;
                            case 3:
                                questionItem.stageTxt = "Subtopic: Software Configuration";
                                break;
                        }
                        break;

                    case 4:
                        questionItem.worldTxt = "Topic: Implementation";
                        switch (subtopic)
                        {
                            case 1:
                                questionItem.stageTxt = "Subtopic: Software Testing Techniques and Strategies";
                                break;
                            case 2:
                                questionItem.stageTxt = "Subtopic: Testing Application";
                                break;
                            case 3:
                                questionItem.stageTxt = "Subtopic: Software Testing";
                                break;
                        }
                        break;

                    case 5:
                        questionItem.worldTxt = "Topic: Maintenance";
                        switch (subtopic)
                        {
                            case 1:
                                questionItem.stageTxt = "Subtopic: Software Management";
                                break;
                            case 2:
                                questionItem.stageTxt = "Subtopic: Software Configuration";
                                break;
                            case 3:
                                questionItem.stageTxt = "Subtopic: Quality Management";
                                break;
                        }
                        break;

                    default:
                        break;
                }


                questionItem.assignmentTxt = "";
                //questionItem.worldTxt = "Topic: " + questionList[i].world.ToString();
                //questionItem.stageTxt = "Subtopic: " + questionList[i].stage.ToString();
                questionItem.difficultyTxt = "Difficulty: \n" + questionList[i].difficulty;
                questionItem.questionIDTxt = "Qn No: " + questionList[i].qnID.ToString();
                questionItem.questionTxt = questionList[i].question;
                questionItem.o1Txt = "1. " + questionList[i].option1;
                questionItem.o2Txt = "2. " + questionList[i].option2;
                questionItem.o3Txt = "3. " + questionList[i].option3;
                questionItem.o4Txt = "4. " + questionList[i].option4;
                questionItem.answerTxt = "Answer: " + questionList[i].answer.ToString();

                GameObject newContainer = questionObjectPool.GetObject();

                //newContainer.transform.SetParent(contentPanel); //Sets "newParent" as the new parent of the player GameObject
                newContainer.transform.SetParent(contentPanel, false); //Same as above, except this makes the player keep its local orientation rather than its global orientation.

                QuestionContainer questionContainer = newContainer.GetComponent<QuestionContainer>();
                print("Before QuestionContainer SetUp");
                print("QUESTION ITEM COUNT: " + questionItemList);
                questionContainer.Setup(questionItem, this);
                print("After QuestionContainer SetUp");
            }
        }

    }
}
