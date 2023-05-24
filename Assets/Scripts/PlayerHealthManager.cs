using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float _maxHealth;
    private float _health;
    public static PlayerHealthManager instance;

    private void Start()
    {
        instance = this;
        _health = _maxHealth;
        GameManager.OnHealthChanged?.Invoke(_health);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        GameManager.OnHealthChanged?.Invoke(_health);
        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Time.timeScale = 0;
        GameManager.OnPlayerDied?.Invoke();
    }
}
