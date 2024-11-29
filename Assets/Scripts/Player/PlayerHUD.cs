using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private WeaponUI weaponUI;

    public void UpdateHealth()
    {

    }

    public void UpdateWeaponUI(WeaponSO weapon)
    {
        weaponUI.UpdateInfo(weapon.weaponIcon, weapon.magazineSize, weapon.magazineCount);
    }
}
