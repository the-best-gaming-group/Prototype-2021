using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text healthText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        
        fill.color = gradient.Evaluate(1f);
        healthText.text = health + " \\ " + health;
    }

    public void SetHealth(int health, int oldHealth)
    {
        // Debug.Log("Setting health to: " + health);

        // Ensure that health is within the valid range (0 to slider.maxValue)
        health = Mathf.Clamp(health, 0, (int)slider.maxValue);

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        healthText.text = health + " \\ 100";

        if(FloatingTextPrefab && health < oldHealth)
        {
            int damage = oldHealth - health;
            showFloatingText(damage);
        }
            
    }

    public void showFloatingText(int damage)
    {
        var go = Instantiate(FloatingTextPrefab, new Vector2(healthText.transform.position.x, healthText.transform.position.y) , Quaternion.identity);
        go.GetComponent<TextMesh>().text = "-" + damage.ToString();
    }

}
