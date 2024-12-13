using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public static AnimationEventManager Instance;

    private WeaponManager weaponManager;
    private WeaponShooting weaponShooting;
    private Inventory inventory;

    private Animator anim;

    void Awake()
    {
        if(Instance == null) Instance = this;

        weaponShooting = GetComponentInParent<WeaponShooting>();
        anim = GetComponent<Animator>();
        inventory = GetComponentInParent<Inventory>();
    }

    private void Start()
    {
        weaponManager = WeaponManager.Instance;
    }

    public void DestroyWeapon()
    {
        Destroy(weaponManager.currentWeaponObject);
    }

    public void InstantiateWeapon()
    {
        weaponManager.currentWeaponObject = Instantiate(inventory.GetWeapon(weaponManager.currentWeaponIndex).weaponPrefab, weaponManager.weaponHolderR);
        weaponManager.currentWeaponAnim = weaponManager.currentWeaponObject.GetComponent<Animator>();
        weaponManager.muzzleEffectPosition = weaponManager.currentWeaponObject.transform.GetChild(0);
    }

    //Avoid shooting when switching
    public void CheckSwitchingDone()
    {
        weaponManager.isSwitching = false;
    }

    public void StartReloading()
    {
        weaponShooting.isReloading = true;
    }

    public void EndReloading()
    {
        weaponShooting.isReloading = false;
    }

    public void SetWeaponType(WeaponSO weaponSO)
    {
        anim.SetInteger("WeaponType", (int)weaponSO.weaponType);
    }

    public void SetTriggerUnequip()
    {
        anim.SetTrigger("Unequip");
    }
}
