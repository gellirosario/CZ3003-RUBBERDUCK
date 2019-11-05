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
        //List<Assignment> filteredAssignmentQnList = new List<Assignment>();

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

        /*for (int i = 0; i < assignmentQuestionListCount; i++)
        {
            print("outside filteredAssignmentQnList");
            print(" Index of assignmentQnList[i].assignmentName: " + i);
            print(" Index of assignmentQnList[i+1].assignmentName: " + (i + 1).ToString());
            print(" AssignmentQnList[i].assignmentName: " + assignmentQnList[i].assignmentName);
            print(" AssignmentQnList[i+1].assignmentName: " + assignmentQnList[i+1].assignmentName);
            // check if the list contains the same assignment names from the list of all assignment names

            if (assignmentQnList[i].assignmentName != assignmentQnList[i + 1].assignmentName)
            {
                filteredAssignmentQnList.Add(new Assignment());
                filteredAssignmentQnList[i].assignmentName = assignmentQnList[i].assignmentName;
                filteredAssignmentQnList[i].qnID = assignmentQnList[i].qnID;
                filteredAssignmentQnList[i].world = assignmentQnList[i].world;
                filteredAssignmentQnList[i].stage = assignmentQnList[i].stage;
                filteredAssignmentQnList[i].difficulty = assignmentQnList[i].difficulty;
                filteredAssignmentQnList[i].question = assignmentQnList[i].question;
                filteredAssignmentQnList[i].answer = assignmentQnList[i].answer;
                filteredAssignmentQnList[i].option1 = assignmentQnList[i].option1;
                filteredAssignmentQnList[i].option2 = assignmentQnList[i].option2;
                filteredAssignmentQnList[i].option3 = assignmentQnList[i].option3;
                filteredAssignmentQnList[i].option4 = assignmentQnList[i].option4;
                break;
            }
                

            print("in filteredAssignmentQnList");
            Assignment assignment = new Assignment();
            filteredAssignmentQnList.Add(assignment);
            print(" Index of assignmentQnList[i].assignmentName: " + assignmentQnList[i].assignmentName);
            filteredAssignmentQnList[i].assignmentName = assignmentQnList[i].assignmentName;
            filteredAssignmentQnList[i].qnID = assignmentQnList[i].qnID;
            filteredAssignmentQnList[i].world = assignmentQnList[i].world;
            filteredAssignmentQnList[i].stage = assignmentQnList[i].stage;
            filteredAssignmentQnList[i].difficulty = assignmentQnList[i].difficulty;
            filteredAssignmentQnList[i].question = assignmentQnList[i].question;
            filteredAssignmentQnList[i].answer = assignmentQnList[i].answer;
            filteredAssignmentQnList[i].option1 = assignmentQnList[i].option1;
            filteredAssignmentQnList[i].option2 = assignmentQnList[i].option2;
            filteredAssignmentQnList[i].option3 = assignmentQnList[i].option3;
            filteredAssignmentQnList[i].option4 = assignmentQnList[i].option4;
       
        }

        print("Filtered Assignment Data Size: " + filteredAssignmentQnList.Count);

        for (int i = 0; i < filteredAssignmentQnList.Count; i++)
        {
            print(i);
            print(filteredAssignmentQnList[i].assignmentName + "Qn id:" + filteredAssignmentQnList[i].qnID + "world: " +filteredAssignmentQnList[i].world + filteredAssignmentQnList[i].stage + filteredAssignmentQnList[i].difficulty + filteredAssignmentQnList[i].question +
                    filteredAssignmentQnList[i].answer + filteredAssignmentQnList[i].option1 + filteredAssignmentQnList[i].option2 + filteredAssignmentQnList[i].option3 + filteredAssignmentQnList[i].option4);
        }

        return filteredAssignmentQnList;*/

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