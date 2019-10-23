using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;


    public void Setup(HealthSystem healthSystem)
    {

        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        //load text
        UpdateHealthbarText(healthSystem.GetHealth(), healthSystem.GetMaxHealth());

    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {

        //changed filled healthbar size
        transform.Find("HealthbarFilled").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);

        //RectTransform bar = transform.Find("HealthbarFilled").gameObject.GetComponent<RectTransform>();
        //bar.localScale = new Vector3(healthSystem.GetHealthPercent(), 1);

        //change text
        UpdateHealthbarText(healthSystem.GetHealth(), healthSystem.GetMaxHealth());

    }

    private void UpdateHealthbarText(int current, int max)
    {
        transform.Find("HealthbarText").GetComponent<TextMeshProUGUI>().text = current.ToString() + " / " + max.ToString();
    }

}
