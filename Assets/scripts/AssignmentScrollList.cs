using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string assignmentTxt;
    //public string questionName;

    /*public Item()
    {
    }*/
}

public class AssignmentScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public List<string> assignmentNameList = new List<string>();
    public Transform contentPanel;
    public AssignmentScrollList createAssignment;
    public SimpleObjectPool buttonObjectPool;


    // Use this for initialization
    void Start()
    {
        print("reach start of assignmentscrolllist");
        assignmentNameList = retrieveAssignmentNamesFromAssignmentController();

        // Wait for 5 seconds
        StartCoroutine(Timer(5));
        RefreshDisplay();

    }

    IEnumerator Timer(int secs)
    {
        yield return new UnityEngine.WaitForSeconds(secs);
    }

    private List<string> retrieveAssignmentNamesFromAssignmentController()
    {
        List<string> assignmentNamesList = new List<string>(PlayerPrefs.GetInt("AssignmentNameListCount"));

        if (assignmentNamesList.Capacity == 0)
            return null;

        int count = assignmentNamesList.Capacity;

        print("In assignmentscrolllist count: " + count);

        for (int i = 0; i < count; i++)
        {
            assignmentNamesList.Add("");

            assignmentNamesList[i] = PlayerPrefs.GetString("AssignmentNameList" + i);

        }

        for (int i = 0; i < count; i++)
        {
            print("Assignment Name List contents");
            print(assignmentNamesList[i]);
        }
        return assignmentNamesList;
    }

    void RefreshDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    private void RemoveButtons()
    {
        itemList.Clear();
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        
        print("local assignmentlist: " + assignmentNameList.Capacity);
        if (assignmentNameList.Capacity == 0)
            return;
        print("reach here");
        itemList.Capacity = assignmentNameList.Capacity;
        print("Item List capacity count: " + itemList.Capacity);
        int itemCount = itemList.Capacity;
        for (int i = 0; i < itemCount; i++)
        {
            //Item item = itemList[i];
            Item item = new Item();
            itemList.Add(item);
            item.assignmentTxt = assignmentNameList[i];
            //item.questionName = assignmentList[i].question;
            GameObject newButton = buttonObjectPool.GetObject();

            //newButton.transform.SetParent(contentPanel); //Sets "newParent" as the new parent of the player GameObject
            newButton.transform.SetParent(contentPanel, false); //Same as above, except this makes the player keep its local orientation rather than its global orientation.

            newButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(item.assignmentTxt));
            AssignmentButton assignmentButton = newButton.GetComponent<AssignmentButton>();

            assignmentButton.Setup(item, this);
            print("PLACED BUTTONS");
        }
    }

    public void OnButtonClick(string assignmentName)
    {
        Debug.Log(assignmentName);
        PlayerPrefs.SetString("assignNameCode", assignmentName);
        SceneSwitcher sceneSwitch = gameObject.AddComponent<SceneSwitcher>();
        sceneSwitch.LoadNextScene("AssignmentQuestionList");
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