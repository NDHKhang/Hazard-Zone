using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "GameConfiguration/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float chaseRange { get; private set; }
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float health { get; private set; }
}
