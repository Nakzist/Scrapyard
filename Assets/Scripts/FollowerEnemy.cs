using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowerEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float range = 1f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float delayBetweenAttacks = 5;
    [SerializeField] private float _maxHealth = 5;
    private float _health;
    private float lastAttackTime;
    private bool canMove = true;
    private Vector3 velocity;
    public bool isQueen;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        velocity = Vector3.zero;
        if (Vector3.Distance(PlayerInputHandler.instance.gameObject.transform.position, transform.position) <= range && lastAttackTime + delayBetweenAttacks <= Time.time)
        {
            StartCoroutine(Attack());
        }
        else
        {
            if (canMove)
            {
                velocity = (PlayerInputHandler.instance.gameObject.transform.position - transform.position).normalized *
                           moveSpeed;
                velocity.y = 0;
            }
        }
        GetComponent<Rigidbody>().velocity = velocity + new Vector3(0,GetComponent<Rigidbody>().velocity.y,0);
    }

    IEnumerator Attack()
    {
        canMove = false;
        lastAttackTime = Time.time;
        PlayerHealthManager.instance.TakeDamage(damage);
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.aliveEnemies.Remove(gameObject);
        if (isQueen)
        {
            GameManager.OnQueenDied?.Invoke();
        }
        GameManager.OnEnemyDied?.Invoke();
        Debug.Log("die");
        Destroy(gameObject);
    }
}
