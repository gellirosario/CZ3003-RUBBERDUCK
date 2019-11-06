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
        print("Local assignmentQuestionList: " + assignmentQuestionList.Capacity);
        if (assignmentQuestionList.Capacity == 0)
            return;
        questionItemList.Capacity = assignmentQuestionList.Capacity;
        print("Item List capacity count: " + questionItemList.Capacity);
        int itemQuestionCount = questionItemList.Capacity;

        for (int i = 0; i < itemQuestionCount; i++)
        {
            if(assignmentQuestionList[i].assignmentName == PlayerPrefs.GetString("assignNameCode"))
            {
                QuestionItem questionItem = new QuestionItem();
                questionItemList.Add(questionItem);

                questionItem.assignmentTxt = "Name: " + assignmentQuestionList[i].assignmentName;
                questionItem.worldTxt = "Topic: " + assignmentQuestionList[i].world.ToString();
                questionItem.stageTxt = "Subtopic: " + assignmentQuestionList[i].stage.ToString();
                questionItem.difficultyTxt = assignmentQuestionList[i].difficulty;
                questionItem.questionIDTxt = "Qn No.: " + assignmentQuestionList[i].qnID.ToString();
                questionItem.questionTxt = assignmentQuestionList[i].question;
                questionItem.o1Txt = assignmentQuestionList[i].option1;
                questionItem.o2Txt = assignmentQuestionList[i].option2;
                questionItem.o3Txt = assignmentQuestionList[i].option3;
                questionItem.o4Txt = assignmentQuestionList[i].option4;
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