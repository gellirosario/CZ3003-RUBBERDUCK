using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeController : MonoBehaviour
{
    private int selectIndex;
    [Header("UI reference")]
    [SerializeField] private Image characterSplash;
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();

    private void UpdateCharacterSelectionUI()
    {
        characterSplash.sprite = characterList[selectIndex].splash;
        //haracterSplash.sprite = characterList[selectIndex].splash;
        Debug.Log(string.Format("uodate"));
    }
    [System.Serializable]
    public class CharacterSelectObject
    {
        public string characterName;
        public Sprite splash;
    }

    public void RightArrow()
    {
        selectIndex++;
        if (selectIndex == characterList.Count)
            selectIndex = 0;
        //Debug.Log(string.Format("right"));
        UpdateCharacterSelectionUI();
    }
    public void LeftArrow()
    {
        selectIndex--;
        if (selectIndex <0)
            selectIndex = characterList.Count -1;
        //Debug.Log(string.Format("left"));
        UpdateCharacterSelectionUI();
    }
    public void Confirm()
    {
        Debug.Log(string.Format("selected {0}:{1} ",selectIndex,characterList[selectIndex].characterName));
    }
    // Start is called before the first frame update
    private void Start()
    {
        UpdateCharacterSelectionUI();
        Debug.Log(string.Format("start"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
