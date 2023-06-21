using System;
using System.Collections;
using System.Collections.Generic;
using _GAME_.Scripts.Enemy;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private void OnEnable()
    {
        Boss.OnBossTakeDamage += ShowBossHealth;
    }
    
    private void OnDisable()
    {
        Boss.OnBossTakeDamage -= ShowBossHealth;
    }
    
    private void ShowBossHealth(float currentHealth, float maxHealth)
    {
        GetComponent<Slider>().value = currentHealth / maxHealth;
    }
}
