using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string assignmentName;
    //public string questionName;

    /*public Item()
    {
    }*/
}

public class AssignmentScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public List<Assignment> assignmentList = new List<Assignment>();
    public Transform contentPanel;
    public AssignmentScrollList createAssignment;
    public SimpleObjectPool buttonObjectPool;


    // Use this for initialization
    void Start()
    {

        assignmentList = retrieveAssignmentDataFromAssignmentController();

        // Wait for 10 seconds
        StartCoroutine(Timer(10));
        RefreshDisplay();

    }

    IEnumerator Timer(int secs)
    {
        yield return new UnityEngine.WaitForSeconds(secs);
    }

    private List<Assignment> retrieveAssignmentDataFromAssignmentController()
    {
        List<Assignment> assignmentList2 = new List<Assignment>(PlayerPrefs.GetInt("ALCount"));
        int assignmentListCount = assignmentList2.Capacity;

        print("In assignmentscrolllist count: " + assignmentListCount);

        for (int i = 0; i < assignmentListCount; i++)
        {
            Assignment assignment = new Assignment();
            assignmentList2.Add(assignment);
            assignmentList2[i].assignmentName = PlayerPrefs.GetString("AssignmentList(assignmentName)" + i);
            assignmentList2[i].qnID = PlayerPrefs.GetInt("AssignmentList(qnID)" + i);
            assignmentList2[i].world = PlayerPrefs.GetInt("AssignmentList(world)" + i);
            assignmentList2[i].stage = PlayerPrefs.GetInt("AssignmentList(stage)" + i);
            assignmentList2[i].difficulty = PlayerPrefs.GetString("AssignmentList(difficulty)" + i);
            assignmentList2[i].question = PlayerPrefs.GetString("AssignmentList(question)" + i);
            assignmentList2[i].answer = PlayerPrefs.GetInt("AssignmentList(answer)" + i);
            assignmentList2[i].option1 = PlayerPrefs.GetString("AssignmentList(option1)" + i);
            assignmentList2[i].option2 = PlayerPrefs.GetString("AssignmentList(option2)" + i);
            assignmentList2[i].option3 = PlayerPrefs.GetString("AssignmentList(option3)" + i);
            assignmentList2[i].option4 = PlayerPrefs.GetString("AssignmentList(option4)" + i);
        }

        /*for (int i = 0; i < assignmentListCount; i++)
        {
            print("Assignment Data contents");
            print(assignmentList2[i].assignmentName + assignmentList2[i].qnID + assignmentList2[i].world + assignmentList2[i].stage + assignmentList2[i].difficulty + assignmentList2[i].question +
                    assignmentList2[i].answer + assignmentList2[i].option1 + assignmentList2[i].option2 + assignmentList2[i].option3 + assignmentList2[i].option4);
        }*/
        return assignmentList2;
    }

    void RefreshDisplay()
    {
        //RemoveButtons();
        AddButtons();
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        //print("local assignmentlist: " + assignmentList.Capacity);
        itemList.Capacity = assignmentList.Capacity;
        //print("Item List capacity count: " + itemList.Capacity);
        int itemCount = itemList.Capacity;
        for (int i = 0; i < itemCount; i++)
        {
            //Item item = itemList[i];
            Item item = new Item();
            itemList.Add(item);
            item.assignmentName = assignmentList[i].assignmentName;
            //item.questionName = assignmentList[i].question;
            GameObject newButton = buttonObjectPool.GetObject();

            //newButton.transform.SetParent(contentPanel); //Sets "newParent" as the new parent of the player GameObject
            newButton.transform.SetParent(contentPanel, false); //Same as above, except this makes the player keep its local orientation rather than its global orientation.

            AssignmentButton assignmentButton = newButton.GetComponent<AssignmentButton>();
            assignmentButton.Setup(item, this);
        }
    }

    /*public void addAssignment(Item item)
    {
        if (otherShop.gold >= item.price)
        {
            gold += item.price;
            otherShop.gold -= item.price;

            AddItem(item, otherShop);
            RemoveItem(item, this);

            RefreshDisplay();
            otherShop.RefreshDisplay();
            Debug.Log("enough gold");

        }
        Debug.Log("attempted");
    }*/

    void AddItem(Item itemToAdd, AssignmentScrollList assignmentList)
    {
        assignmentList.itemList.Add(itemToAdd);
    }

    private void RemoveItem(Item itemToRemove, AssignmentScrollList assignmentList)
    {
        for (int i = assignmentList.itemList.Count - 1; i >= 0; i--)
        {
            if (assignmentList.itemList[i] == itemToRemove)
            {
                assignmentList.itemList.RemoveAt(i);
            }
        }
    }
}