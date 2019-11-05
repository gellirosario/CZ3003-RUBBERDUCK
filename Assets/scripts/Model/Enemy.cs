using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int randomEnemy;
    public Animator enemy1Anim;
    public Animator enemy2Anim;
    public Animator enemy3Anim;

    public Enemy()
    {

    }
    public Enemy(int random, Animator enemy1Animator, Animator enemy2Animator, Animator enemy3Animator)
    {
        randomEnemy = random;
        enemy1Anim = enemy1Animator;
        enemy2Anim = enemy2Animator;
        enemy3Anim = enemy3Animator;
    }

    public void Idle()
    {
        switch (randomEnemy)
        {
            case 0:
                enemy1Anim.SetTrigger("Idle");
                break;
            case 1:
                enemy1Anim.SetTrigger("Idle");
                break;
            case 2:
                enemy1Anim.SetTrigger("Idle");
                break;
        }
    }

    public void TakeDamage()
    {
        switch (randomEnemy)
        {
            case 0:
                enemy1Anim.SetTrigger("Damage");
                break;
            case 1:
                enemy2Anim.SetTrigger("Damage");
                break;
            case 2:
                enemy3Anim.SetTrigger("Damage");
                break;
        }
    }

    public void Down()
    {
        switch (randomEnemy)
        {
            case 0:
                enemy1Anim.SetTrigger("Down");
                break;
            case 1:
                enemy2Anim.SetTrigger("Down");
                break;
            case 2:
                enemy3Anim.SetTrigger("Down");
                break;
        }
    }

    public void Swinging()
    {
        switch (randomEnemy)
        {
            case 0:
                enemy1Anim.SetTrigger("Swinging");
                break;
            case 1:
                enemy2Anim.SetTrigger("Swinging");
                break;
            case 2:
                enemy3Anim.SetTrigger("Swinging");
                break;
        }
    }
}
