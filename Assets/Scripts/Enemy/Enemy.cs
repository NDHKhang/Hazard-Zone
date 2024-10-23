using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO enemy;

    [HideInInspector] public float health;
    [HideInInspector] public float damage;
    [HideInInspector] public float chaseRange;

    void Start()
    {
        if (enemy == null) return;

        health = enemy.health;
        damage = enemy.damage;
        chaseRange = enemy.chaseRange;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (enemy == null) return;
        Gizmos.color = new Color(1, 1, 0, 1F);
        Gizmos.DrawWireSphere(transform.position, enemy.chaseRange);
    }
}
