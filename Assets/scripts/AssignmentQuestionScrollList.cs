using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionItem
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

public class AssignmentQuestionScrollList : MonoBehaviour
{
    public List<QuestionItem> questionItemList;
    public List<Assignment> assignmentQuestionList = new List<Assignment>();
    public Transform contentPanel;
    public SimpleObjectPool questionContainerObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        assignmentQuestionList = retrieveAssignmentDataFromAssignmentController();
        RefreshDisplay();
    }

    private List<Assignment> retrieveAssignmentDataFromAssignmentController()
    {
        List<Assignment> assignmentQnList = new List<Assignment>(PlayerPrefs.GetInt("AssignmentQuestionListCount"));

        if (assignmentQnList.Capacity == 0)
            return null;

        int assignmentQuestionListCount = assignmentQnList.Capacity;

        print("In assignmentscrolllist count: " + assignmentQuestionListCount);

        for (int i = 0; i < assignmentQuestionListCount; i++)
        {
            Assignment assignment = new Assignment();
            assignmentQnList.Add(assignment);
            assignmentQnList[i].assignmentName = PlayerPrefs.GetString("AssignmentList(assignmentName)" + i);
            assignmentQnList[i].qnID = PlayerPrefs.GetInt("AssignmentList(qnID)" + i);
            assignmentQnList[i].world = PlayerPrefs.GetInt("AssignmentList(world)" + i);
            assignmentQnList[i].stage = PlayerPrefs.GetInt("AssignmentList(stage)" + i);
            assignmentQnList[i].difficulty = PlayerPrefs.GetString("AssignmentList(difficulty)" + i);
            assignmentQnList[i].question = PlayerPrefs.GetString("AssignmentList(question)" + i);
            assignmentQnList[i].answer = PlayerPrefs.GetInt("AssignmentList(answer)" + i);
            assignmentQnList[i].option1 = PlayerPrefs.GetString("AssignmentList(option1)" + i);
            assignmentQnList[i].option2 = PlayerPrefs.GetString("AssignmentList(option2)" + i);
            assignmentQnList[i].option3 = PlayerPrefs.GetString("AssignmentList(option3)" + i);
            assignmentQnList[i].option4 = PlayerPrefs.GetString("AssignmentList(option4)" + i);


        }

        return assignmentQnList;
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
            questionContainerObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddQuestionContainers()
    {
        if (assignmentQuestionList.Capacity == 0)
            return;
        print("Local assignmentQuestionList: " + assignmentQuestionList.Capacity);
        questionItemList.Capacity = assignmentQuestionList.Capacity;
        print("Item List capacity count: " + questionItemList.Capacity);
        int itemQuestionCount = questionItemList.Capacity;

        for (int i = 0; i < itemQuestionCount; i++)
        {
            if (assignmentQuestionList[i].assignmentName == PlayerPrefs.GetString("assignNameCode"))
            {
                QuestionItem questionItem = new QuestionItem();
                questionItemList.Add(questionItem);

                int topic = assignmentQuestionList[i].world;
                int subtopic = assignmentQuestionList[i].stage;

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





                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                questionItem.assignmentTxt = "Assignment Name: \n" + assignmentQuestionList[i].assignmentName;
                //questionItem.worldTxt = "Topic: " + assignmentQuestionList[i].world.ToString();
                //questionItem.stageTxt = "Subtopic: " + assignmentQuestionList[i].stage.ToString();
                questionItem.difficultyTxt = "Difficulty: \n" + assignmentQuestionList[i].difficulty;
                questionItem.questionIDTxt = "Qn No: " + assignmentQuestionList[i].qnID.ToString();
                questionItem.questionTxt = assignmentQuestionList[i].question;
                questionItem.o1Txt = "1. " + assignmentQuestionList[i].option1;
                questionItem.o2Txt = "2. " + assignmentQuestionList[i].option2;
                questionItem.o3Txt = "3. " + assignmentQuestionList[i].option3;
                questionItem.o4Txt = "4. " + assignmentQuestionList[i].option4;
                questionItem.answerTxt = "Answer: " + assignmentQuestionList[i].answer.ToString();

                GameObject newContainer = questionContainerObjectPool.GetObject();

                //newContainer.transform.SetParent(contentPanel); //Sets "newParent" as the new parent of the player GameObject
                newContainer.transform.SetParent(contentPanel, false); //Same as above, except this makes the player keep its local orientation rather than its global orientation.

                AssignmentQuestionContainer assignmentQuestionContainer = newContainer.GetComponent<AssignmentQuestionContainer>();
                assignmentQuestionContainer.Setup(questionItem, this);
            }

        }
    }
}