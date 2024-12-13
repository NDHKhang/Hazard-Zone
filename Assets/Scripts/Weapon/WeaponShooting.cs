using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Location storing spawn VFX")]
    [SerializeField] private Transform vfxParent;

    private Animator anim;

    //References
    private StarterAssetsInputs _input;
    private WeaponManager weaponManager;
    private Inventory inventory;
    private PlayerHUD playerHUD;

    // fire rate & check can shoot
    private float timeLastShot = 0f;
    public bool canShoot;
    public bool isReloading = false;

    //Ammo
    private int primaryCurrentAmmo;
    private int primaryCurrentTotalAmmo;

    private int secondaryCurrentAmmo;
    private int secondaryCurrentTotalAmmo;

    //Checking empty ammo
    private bool isPrimaryMagazineEmpty;
    private bool isSecondaryMagazineEmpty;

    void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        anim = GetComponentInChildren<Animator>();
        inventory =  GetComponent<Inventory>();
        playerHUD = GetComponent<PlayerHUD>();
    }

    private void Start()
    {
        weaponManager = WeaponManager.Instance;
        canShoot = true;
    }
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (_input.shoot)
        {
            HandleShoot();
        }

        if (_input.reload)
        {
            HandleReload(weaponManager.currentWeaponIndex);
        }
    }

    private void HandleShoot()
    {   
        CheckCanShoot(weaponManager.currentWeaponIndex);

        //if cannot shoot & is reloading -> dont shoot
        if (!canShoot || isReloading)
        {
            //TODO: Add cant shoot sound
            return;
        }
        //if player is switching weapon -> cant shoot
        if (weaponManager.isSwitching) return;

        WeaponSO weaponSO = inventory.GetWeapon(weaponManager.currentWeaponIndex);

        if (Time.time >= timeLastShot + weaponSO.fireRate)
        {
            Shoot(weaponSO);
            timeLastShot = Time.time;
            ShootAnimation(weaponSO);
            UseAmmo((int)weaponSO.weaponStyle, 1, 0);
        }

        // for pistol & not rifle gun type
        if (!weaponSO.isAutomatic)
        {
            _input.shoot = false;
        }
    }

    private void CheckCanShoot(int slot)
    {
        if (slot == 0)
        {
            if (isPrimaryMagazineEmpty)
                canShoot = false;
            else
                canShoot = true;
        }

        if (slot == 1)
        {
            if (isSecondaryMagazineEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
    }

    public void Shoot(WeaponSO weaponSO)
    {
        RaycastHit hit; //Variable for storing the information of WHAT WE HIT

        //Shooting
        //When hit the enemy, decrease the enemy's health
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponSO.range))
        {
            Debug.Log("Hit" + hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponSO.damage);
            }

            GameObject impact = Instantiate(weaponSO.hitEffect, hit.point, Quaternion.LookRotation(hit.normal), vfxParent);
            Destroy(impact, 1f);
        }

        GameObject muzzle = Instantiate(weaponSO.muzzleEffect, weaponManager.muzzleEffectPosition);
        Destroy(muzzle, 1f);
    }

    private void ShootAnimation(WeaponSO weaponSO)
    {
        anim.CrossFade(weaponSO.weaponName + ".Shoot", 0.1f, -1, 0f);
    }

    private void HandleReload(int slot)
    {
        if (!isReloading && CheckCanReload(slot))
        {
            if (slot == 0)
            {
                Reload(slot, ref primaryCurrentAmmo, ref primaryCurrentTotalAmmo);
                isPrimaryMagazineEmpty = false;
                CheckCanShoot(slot);
            }

            if (slot == 1)
            {
                Reload(slot, ref secondaryCurrentAmmo, ref secondaryCurrentTotalAmmo);
                isSecondaryMagazineEmpty = false;
                CheckCanShoot(slot);
            }
        }
        _input.reload = false;
    }

    private bool CheckCanReload(int slot)
    {
        if (slot == 0)
        {
            if (primaryCurrentAmmo == inventory.GetWeapon(slot).magazineSize || primaryCurrentTotalAmmo <= 0)
                return false;
            else 
                return true;
        }
        if (slot == 1)
        {
            if (secondaryCurrentAmmo == inventory.GetWeapon(slot).magazineSize || secondaryCurrentTotalAmmo <= 0)
                return false;
            else 
                return true;
        }

        return false;
    }

    private void Reload(int slot, ref int currentAmmo, ref int totalAmmo)
    {
        //if ammo is still full -> return
        if (currentAmmo == inventory.GetWeapon(slot).magazineSize)
        {
            return;
        }

        int ammoToReload = inventory.GetWeapon(slot).magazineSize - currentAmmo;

        //Only reload if have enough ammo
        if (ammoToReload <= totalAmmo)
        {
            //Add ammo to magazine, reduce ammo from total ammo
            AddAmmo(slot, ammoToReload, 0);
            UseAmmo(slot, 0, ammoToReload);
        }
        else //if total ammo is not enough, reload all remaning ammo
        {
            ammoToReload = totalAmmo;
            AddAmmo(slot, ammoToReload, 0);
            UseAmmo(slot, 0, ammoToReload);
        }
        WeaponSO weaponSO = inventory.GetWeapon(weaponManager.currentWeaponIndex);
        ReloadAnimation(weaponSO);
    }

    private void ReloadAnimation(WeaponSO weaponSO)
    {
        anim.CrossFade(weaponSO.weaponName + ".Reload", 0.1f, -1, 0f);
        weaponManager.currentWeaponAnim.SetTrigger("Reload");
    }

    public void InitAmmo(int slot, WeaponSO weapon)
    {
        if(slot == 0)
        {
            primaryCurrentAmmo = weapon.magazineSize;
            primaryCurrentTotalAmmo = weapon.totalAmmo;
        }

        if(slot == 1)
        {
            secondaryCurrentAmmo = weapon.magazineSize;
            secondaryCurrentTotalAmmo = weapon.totalAmmo;
        }
    }

    private void UseAmmo(int slot, int ammoUsed, int totalAmmoUsed)
    {
        if(slot == 0)
        {
            primaryCurrentAmmo -= ammoUsed;
            primaryCurrentTotalAmmo -= totalAmmoUsed;
            playerHUD.UpdateAmmoUI(primaryCurrentAmmo, primaryCurrentTotalAmmo);

            //Check if there's enough ammo before shooting
            if (primaryCurrentAmmo <= 0)
            {
                isPrimaryMagazineEmpty = true;
                CheckCanShoot(weaponManager.currentWeaponIndex);
            }
        }

        if (slot == 1)
        {
            secondaryCurrentAmmo -= ammoUsed;
            secondaryCurrentTotalAmmo -= totalAmmoUsed;
            playerHUD.UpdateAmmoUI(secondaryCurrentAmmo, secondaryCurrentTotalAmmo);

            //Check if there's enough ammo before shooting
            if (secondaryCurrentAmmo <= 0)
            {
                isSecondaryMagazineEmpty = true;
                CheckCanShoot(weaponManager.currentWeaponIndex);
            }
        }
    }

    private void AddAmmo(int slot, int ammoAdded, int totalAmmoAdded)
    {
        if (slot == 0)
        {
            primaryCurrentAmmo += ammoAdded;
            primaryCurrentTotalAmmo += totalAmmoAdded;
            playerHUD.UpdateAmmoUI(primaryCurrentAmmo, primaryCurrentTotalAmmo);
        }

        if (slot == 1)
        {
            secondaryCurrentAmmo += ammoAdded;
            secondaryCurrentTotalAmmo += totalAmmoAdded;
            playerHUD.UpdateAmmoUI(secondaryCurrentAmmo, secondaryCurrentTotalAmmo);
        }
    }
}
