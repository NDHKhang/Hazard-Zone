using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "GameConfiguration/Weapon")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public float damage { get; private set; }
    [field: SerializeField] public float fireRate { get; private set; }

}
