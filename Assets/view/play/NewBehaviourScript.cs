using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    //  public Transform pfHealthBar;

    public Button b1;

    public HealthBar healthbar;



    // Start is called before the first frame update
    public void Start()
    {
        HealthSystem healthSystem = new HealthSystem(50);
        healthbar.Setup(healthSystem);
        b1.onClick.AddListener(() =>
        {

            healthSystem.Damage(10);

           // Debug.Log(healthSystem.GetHealthPercent() + "WOOHOO");

        });
           
         
        
        


     


    }


    public void testButton()
    {




    }




    // Update is called once per frame
   
}
