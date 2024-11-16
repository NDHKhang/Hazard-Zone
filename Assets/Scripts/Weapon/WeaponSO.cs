using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "GameConfiguration/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public float damage;
    public float fireRate;
    public float range;
    public GameObject hitEffect;
    public GameObject muzzleEffect;
    public bool isAutomatic;
    public WeaponType weaponType;
}

public enum WeaponType { Pistol, Rifle };
