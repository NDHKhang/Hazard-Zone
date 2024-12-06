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

    // fire rate & check can shoot
    private float timeLastShot = 0f;
    public bool canShoot = true;

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
    }

    private void Start()
    {
        weaponManager = WeaponManager.Instance;
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

    }

    private void HandleShoot()
    {   
        CheckCanShoot(weaponManager.currentWeaponIndex);
        if (!canShoot)
        {
            //TODO: Add cant shoot sound
            return;
        }
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
        if (_input.shoot)
        {
            anim.CrossFade(weaponSO.weaponName + ".Shoot", 0.1f, -1, 0f);
        }
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
            //Check if empty ammo
            if(primaryCurrentAmmo <= 0)
            {
                isPrimaryMagazineEmpty = true;
                return;
            }

            primaryCurrentAmmo -= ammoUsed;
            primaryCurrentTotalAmmo -= totalAmmoUsed;
        }

        if(slot == 1)
        {
            //Check if empty ammo
            if (secondaryCurrentAmmo <= 0)
            {
                isSecondaryMagazineEmpty = true;
                return;
            }

            secondaryCurrentAmmo -= ammoUsed;
            secondaryCurrentTotalAmmo -= totalAmmoUsed;
        }
    }

    private void CheckCanShoot(int slot)
    {
        if(slot == 0)
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
}
