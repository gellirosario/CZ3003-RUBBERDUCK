using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentQuestionContainer : MonoBehaviour
{
    //public Texture newTexture;
    public RawImage newImage;

    public Text assignmentLabel;
    public Text worldText;
    public Text stageText;
    public Text difficultyText;
    public Text questionIDText;
    public Text questionText, o1Text, o2Text, o3Text, o4Text;
    public Text answerText;

    private QuestionItem questionItem;
    private AssignmentQuestionScrollList scrollList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Setup(QuestionItem currentItem, AssignmentQuestionScrollList currentScrollList)
    {
        questionItem = currentItem;
        scrollList = currentScrollList;

        assignmentLabel.text = questionItem.assignmentTxt;
        worldText.text = questionItem.worldTxt;
        stageText.text = questionItem.stageTxt;
        difficultyText.text = questionItem.difficultyTxt;
        questionIDText.text = questionItem.questionIDTxt;
        questionText.text = questionItem.questionTxt;
        o1Text.text = questionItem.o1Txt;
        o2Text.text = questionItem.o2Txt;
        o3Text.text = questionItem.o3Txt;
        o4Text.text = questionItem.o4Txt;
        answerText.text = questionItem.answerTxt;
    }
}