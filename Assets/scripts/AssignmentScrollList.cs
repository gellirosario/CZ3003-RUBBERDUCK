using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string assignmentName;
    public string questionName;
}

public class AssignmentScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public List<Assignment> assignmentList2 = new List<Assignment>();
    public Transform contentPanel;
    public AssignmentScrollList createAssignment;
    public SimpleObjectPool buttonObjectPool;
    

    // Use this for initialization
    void Start()
    {
        //AssignmentController.Instance.test();
        //assignmentList2 = AssignmentController.Instance.LoadAssignment();
        StartCoroutine(WaitForSecondsWrapper(30));
        print("Message Passed: " + PlayerPrefs.GetString("codeal"));
        int assignmentListCount = PlayerPrefs.GetInt("ALCount");
        assignmentList2.Capacity = assignmentListCount;
        print("In assignmentscrolllist count: " + assignmentListCount);
        for (var i = 0; i < assignmentList2.Count; i++)
        {
            assignmentList2[i].assignmentName = PlayerPrefs.GetString("AssignmentList(assignmentName)");
            assignmentList2[i].qnID = PlayerPrefs.GetInt("AssignmentList(qnID)");
            assignmentList2[i].world = PlayerPrefs.GetInt("AssignmentList(world)");
            assignmentList2[i].stage = PlayerPrefs.GetInt("AssignmentList(stage)");
            assignmentList2[i].difficulty = PlayerPrefs.GetString("AssignmentList(difficulty)");
            assignmentList2[i].question = PlayerPrefs.GetString("AssignmentList(question)");
            assignmentList2[i].answer = PlayerPrefs.GetInt("AssignmentList(answer)");
            assignmentList2[i].option1 = PlayerPrefs.GetString("AssignmentList(option1)");
            assignmentList2[i].option2 = PlayerPrefs.GetString("AssignmentList(option2)");
            assignmentList2[i].option3 = PlayerPrefs.GetString("AssignmentList(option3)");
            assignmentList2[i].option4 = PlayerPrefs.GetString("AssignmentList(option4)");
        }
        /*for (var i = 0; i < assignmentList2; i++)
        {
            if (PlayerPrefs.GetInt("AssignmentList").GetType == typeof(int))
                assignmentList2[i] = PlayerPrefs.GetInt("AssignmentList");

            else if (PlayerPrefs.GetString("AssignmentList").GetType == typeof(String))
                assignmentList2[i] = PlayerPrefs.GetString("AssignmentList");
        }*/

        print("Assignment list count2: " + assignmentList2.Count);
        for (int i = 0; i < assignmentList2.Count; i++)
        {
            print("assignment2lsit contents");
            print(assignmentList2[i].assignmentName + assignmentList2[i].qnID + assignmentList2[i].world + assignmentList2[i].stage + assignmentList2[i].difficulty + assignmentList2[i].question +
                    assignmentList2[i].answer + assignmentList2[i].option1 + assignmentList2[i].option2 + assignmentList2[i].option3 + assignmentList2[i].option4);
        }

        //RefreshDisplay();
        
    }

    IEnumerator WaitForSecondsWrapper(int secs)
    {
        yield return new UnityEngine.WaitForSeconds(secs);
    }
    void RefreshDisplay()
    {
       // RemoveButtons();
        AddButtons();
    }

    /*private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }*/

    


    private void AddButtons()
    {
        itemList.Capacity = assignmentList2.Count;
       
        print("Item List count: " + itemList.Count);
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];
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