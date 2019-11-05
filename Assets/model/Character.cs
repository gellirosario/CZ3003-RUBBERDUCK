using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private int selectedChar;
    public Animator character1Anim;
    public Animator character2Anim;

    public Character()
    {

    }

    public Character(int selected, Animator character1Animator, Animator character2Animator)
    {
        selectedChar = selected;
        character1Anim = character1Animator;
        character2Anim = character2Animator;
    }

    public void SetSelectedCharacter(int selected)
    {
        selectedChar = selected;
    }

    public void IsSick()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetBool("isSick", true);
                break;
            case 1:
                character2Anim.SetBool("isSick", true);
                break;
        }
    }

    public void IsReady()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetBool("isReady", true);
                character1Anim.SetTrigger("Ready");
                break;
            case 1:
                character2Anim.SetBool("isReady", true);
                character2Anim.SetTrigger("Ready");
                break;
        }
    }

    public void Stabbing()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetTrigger("Stabbing");
                break;
            case 1:
                character2Anim.SetTrigger("Stabbing");
                break;
        }
    }

    public void Victory()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetTrigger("Victory");
                break;
            case 1:
                character2Anim.SetTrigger("Victory");
                break;
        }
    }

    public void Ready()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetTrigger("Ready");
                break;
            case 1:
                character2Anim.SetTrigger("Ready");
                break;
        }
    }

    public void TakeDamage()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetTrigger("Damage");
                break;
            case 1:
                character2Anim.SetTrigger("Damage");
                break;
        }
    }

    public void Down()
    {
        switch (selectedChar)
        {
            case 0:
                character1Anim.SetTrigger("Down");
                break;
            case 1:
                character2Anim.SetTrigger("Down");
                break;
        }
    }
}
