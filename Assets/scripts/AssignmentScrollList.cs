using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string assignmentName;
}

public class AssignmentScrollList : MonoBehaviour
{

    public List<Item> itemList;
    public Transform contentPanel;
    public AssignmentScrollList createAssignment;
    public SimpleObjectPool buttonObjectPool;



    // Use this for initialization
    void Start()
    {
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        RemoveButtons();
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