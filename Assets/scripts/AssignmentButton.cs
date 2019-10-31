using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentButton : MonoBehaviour
{
    public Button button;
    public Text assignmentLabel;
    public Text questionLabel;

    private Item item;
    private AssignmentScrollList scrollList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Setup(Item currentItem, AssignmentScrollList currentScrollList)
    {
        item = currentItem;
        assignmentLabel.text = item.assignmentName;
        questionLabel.text = item.questionName;
        scrollList = currentScrollList;

    }


}
