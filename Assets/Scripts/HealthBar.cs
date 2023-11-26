using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthBar;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(float health)
    {
        healthBar.value = health;
    }
}
