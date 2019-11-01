using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    private User currentUser;
    private Player currentPlayer;

    private Mastery m;

    public Text nameText, accTypeText, totalQnAnsweredText;

    public Image w1s1, w1s2, w1s3, w2s1, w2s2, w2s3, w3s1, w3s2, w3s3, w4s1, w4s2, w4s3, w5s1, w5s2, w5s3;

    public Sprite red, black;
    public SpriteRenderer currentCharacter;

    public RuntimeAnimatorController redAnim, blackAnim;
    public Animator currentCharacterAnimator;

    void Start()
    {
        currentUser = ProfileLoader.Instance.userData;
        currentPlayer = ProfileLoader.Instance.playerData;

        m = currentPlayer.mastery;

        nameText.text = currentUser.name;
        accTypeText.text = currentUser.userType;
        totalQnAnsweredText.text = currentPlayer.totalQnAnswered.ToString();
        UpdateAll(m);
        LoadCurrentCharacter(PlayerPrefs.GetInt("CharacterID"));
    }

    private void LoadCurrentCharacter(int characterId)
    {
        Debug.Log("character id: " + characterId);
        switch (characterId)
        {
            case 0:
                currentCharacter.sprite = red;
                currentCharacterAnimator.runtimeAnimatorController = redAnim;
                break;
            case 1:
                currentCharacter.sprite = black;
                currentCharacterAnimator.runtimeAnimatorController = blackAnim;
                break;
            default:
                break;
        }
    }

    private void UpdateAll(Mastery m)
    {
        UpdateStar(m.world1stage1, w1s1);
        UpdateStar(m.world1stage2, w1s2);
        UpdateStar(m.world1stage3, w1s3);
        UpdateStar(m.world2stage1, w2s1);
        UpdateStar(m.world2stage2, w2s2);
        UpdateStar(m.world2stage3, w2s3);
        UpdateStar(m.world3stage1, w3s1);
        UpdateStar(m.world3stage2, w3s2);
        UpdateStar(m.world3stage3, w3s3);
        UpdateStar(m.world4stage1, w4s1);
        UpdateStar(m.world4stage2, w4s2);
        UpdateStar(m.world4stage3, w4s3);
        UpdateStar(m.world5stage1, w5s1);
        UpdateStar(m.world5stage2, w5s2);
        UpdateStar(m.world5stage3, w5s3);
    }


    private void UpdateStar(int masteryLevel, Image star) 
    { 
        star.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, star.rectTransform.rect.width * masteryLevel / 3);
    }

}
