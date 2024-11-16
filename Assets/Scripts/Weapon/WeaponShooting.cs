using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [SerializeField] private Transform weaponHolderR;
    private Animator anim;

    private StarterAssetsInputs _input;
    private WeaponManager weaponManager;

    private float timeLastShot = 0f;
    public bool canShoot = true;

    void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        weaponManager = GetComponent<WeaponManager>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleShoot();
    }

    private void HandleShoot()
    {
        if (!canShoot) return;

        WeaponSO weaponSO = weaponManager.currentWeaponSO;

        if (!weaponSO) return;

        timeLastShot += Time.deltaTime;
        if (!_input.shoot) return;

        if (timeLastShot >= weaponSO.fireRate)
        {
            Shoot(weaponSO);
            timeLastShot = 0f;
            ShootAnimation(weaponSO);
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

            GameObject impact = Instantiate(weaponSO.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
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
}
