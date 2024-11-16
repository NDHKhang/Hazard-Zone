using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "GameConfiguration/Enemy")]
public class EnemySO : ScriptableObject
{
    public float chaseRange;
    public float damage;
    public float health;
}
