using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "GameConfiguration/Weapon")]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public float damage { get; private set; }

    [field: SerializeField] public float fireRate { get; private set; }

    [field: SerializeField] public float range { get; private set; }

    [field: SerializeField] public GameObject hitEffect { get; private set; }

    [field: SerializeField] public bool isAutomatic { get; private set; }


    [field: SerializeField] public WeaponType weaponType { get; private set; }
}

public enum WeaponType { Pistol, Rifle };
