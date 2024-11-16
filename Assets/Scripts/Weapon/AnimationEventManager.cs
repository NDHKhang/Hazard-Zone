using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    private WeaponManager weaponManager;
    private WeaponShooting weaponShooting;

    void Awake()
    {
        weaponManager = GetComponentInParent<WeaponManager>();
        weaponShooting = GetComponentInParent<WeaponShooting>();
    }

    public void DestroyWeapon()
    {
        Destroy(weaponManager.currentWeaponObject);
    }

    public void InstantiateWeapon()
    {
        weaponManager.currentWeaponObject = Instantiate(weaponManager.currentWeaponSO.weaponPrefab, weaponManager.weaponHolderR);
        weaponManager.muzzleEffectPosition = weaponManager.currentWeaponObject.transform.GetChild(0);
    }

    public void CheckSwitchingDone()
    {
        weaponManager.isSwitching = false;
        weaponShooting.canShoot = true;
    }
}
