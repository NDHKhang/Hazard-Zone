using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;

        target.GetComponent<PlayerHealth>().TakeDamage(enemy.damage);
    }
}
