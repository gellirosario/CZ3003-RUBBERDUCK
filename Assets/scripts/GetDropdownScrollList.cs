using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string assignmentName;
}

public class GetDropdownScrollList : MonoBehaviour
{
    public List<Item> itemList;
    public Transform contentPanel;
    public AssignmentScrollList createAssignment;
    public SimpleObjectPool buttonObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
