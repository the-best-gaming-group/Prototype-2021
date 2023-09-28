using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // Debug.Log("Setting health to: " + health);

        // Ensure that health is within the valid range (0 to slider.maxValue)
        health = Mathf.Clamp(health, 0, (int)slider.maxValue);

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
