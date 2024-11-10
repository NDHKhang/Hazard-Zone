using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    private Animator anim;

    private StarterAssetsInputs _input;
    private Weapon currentWeapon;

    private float timeLastShot = 0f;

    void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
        currentWeapon = GetComponentInChildren<Weapon>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleShoot();
    }

    private void HandleShoot()
    {
        timeLastShot += Time.deltaTime;
        if (!_input.shoot) return;

        if (timeLastShot >= weaponSO.fireRate)
        {
            currentWeapon.Shoot(weaponSO);
            timeLastShot = 0f;
            ShootAnimation();
        }
        
        if(!weaponSO.isAutomatic)
        {
            _input.shoot = false;
        }
    }

    private void ShootAnimation()
    {
        if (_input.shoot)
        {
            anim.CrossFade("Fire", 0.1f, -1, 0f);
        }
    }
}
