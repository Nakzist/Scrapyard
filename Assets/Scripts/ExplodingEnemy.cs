using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float range = 1f;
    [SerializeField] private float explodeDamage = 1f;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float delayBetweenAttacks = 5;
    [SerializeField] private float _maxHealth = 5;
    [SerializeField] private GameObject areaPrefab;
    private float _health;
    private float lastAttackTime;
    private bool canMove = true;
    private bool isDead;

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }
        if (Vector3.Distance(PlayerInputHandler.instance.transform.position, transform.position) <= range && lastAttackTime + delayBetweenAttacks <= Time.time)
        {
            Attack();
        }
        else
        {
            Vector3 velocity = (PlayerInputHandler.instance.transform.position - transform.position).normalized * moveSpeed;
            velocity.y = 0;
            transform.LookAt( velocity);
            GetComponent<Rigidbody>().velocity = velocity + new Vector3(0,GetComponent<Rigidbody>().velocity.y,0);
        }
    }

    void Attack()
    {
        Die();
    }

    public void TakeDamage(float damage)
    {
        if(isDead)return;
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GameManager.aliveEnemies.Remove(gameObject);
        GameManager.OnEnemyDied?.Invoke();
        GameObject sphere = Instantiate(areaPrefab, transform.position, Quaternion.identity);
        Destroy(sphere, .3f);
        foreach (var collider in Physics.OverlapSphere(transform.position, 3f))
        {
            if (collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(explodeDamage);
            }
        }
        Destroy(gameObject);
    }
}
