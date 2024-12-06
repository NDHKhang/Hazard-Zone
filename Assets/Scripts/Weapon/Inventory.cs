using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public WeaponSO[] weapons;

    //References
    private WeaponShooting weaponShooting;

    private void Awake()
    {
        weaponShooting = GetComponent<WeaponShooting>();
    }

    private void Start()
    {
        InitVariables();
    }

    public void AddWeapon(WeaponSO newWeapon)
    {
        int weaponIndex = (int)newWeapon.weaponStyle;

        if (weapons[weaponIndex] != null)
        {
            RemoveWeapon(weaponIndex);
        }
        else
        {
            weapons[weaponIndex] = newWeapon;
        }

        weaponShooting.InitAmmo(weaponIndex, newWeapon);
    }

    public void RemoveWeapon(int index)
    {
        weapons[index] = null;
    }

    public WeaponSO GetWeapon(int index)
    {
        return weapons[index];
    }

    private void InitVariables()
    {
        weapons = new WeaponSO[3];
    }
}
