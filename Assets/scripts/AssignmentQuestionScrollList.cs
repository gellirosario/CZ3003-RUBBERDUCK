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

        for (int i = 0; i < assignmentQuestionListCount; i++)
        {
            print("Assignment Data contents");
            print(assignmentQnList[i].assignmentName + assignmentQnList[i].qnID + assignmentQnList[i].world + assignmentQnList[i].stage + assignmentQnList[i].difficulty + assignmentQnList[i].question +
                    assignmentQnList[i].answer + assignmentQnList[i].option1 + assignmentQnList[i].option2 + assignmentQnList[i].option3 + assignmentQnList[i].option4);
        }

        return assignmentQnList;
    }

    void RefreshDisplay()
    {
        AddQuestionContainers();
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
            QuestionItem questionItem = new QuestionItem();
            questionItemList.Add(questionItem);

            questionItem.assignmentTxt = assignmentQuestionList[i].assignmentName;
            questionItem.worldTxt = assignmentQuestionList[i].world.ToString();
            questionItem.stageTxt = assignmentQuestionList[i].stage.ToString();
            questionItem.difficultyTxt = assignmentQuestionList[i].difficulty;
            questionItem.questionIDTxt = assignmentQuestionList[i].qnID.ToString();
            questionItem.questionTxt = assignmentQuestionList[i].question;
            questionItem.o1Txt = assignmentQuestionList[i].option1;
            questionItem.o2Txt = assignmentQuestionList[i].option2;
            questionItem.o3Txt = assignmentQuestionList[i].option3;
            questionItem.o4Txt = assignmentQuestionList[i].option4;
            questionItem.answerTxt = assignmentQuestionList[i].answer.ToString();

            GameObject newContainer = questionContainerObjectPool.GetObject();

            //newContainer.transform.SetParent(contentPanel); //Sets "newParent" as the new parent of the player GameObject
            newContainer.transform.SetParent(contentPanel, false); //Same as above, except this makes the player keep its local orientation rather than its global orientation.

            AssignmentQuestionContainer assignmentQuestionContainer = newContainer.GetComponent<AssignmentQuestionContainer>();
            assignmentQuestionContainer.Setup(questionItem, this);
        }
    }
}