using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    public Animator anim;

    public KeyCode attack1;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Character 
        if (Input.GetKeyDown(attack1))
        {
            anim.SetTrigger("Ready");
        }
    }
}
