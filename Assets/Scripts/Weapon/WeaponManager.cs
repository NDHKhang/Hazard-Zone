using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponSO[] weapons;
    public Transform weaponHolderR;

    private StarterAssetsInputs _input;
    private WeaponShooting weaponShooting;

    [HideInInspector] public GameObject currentWeaponObject;
    [HideInInspector] public WeaponSO currentWeaponSO;
    [HideInInspector] public Transform muzzleEffectPosition;


    private Animator anim;
    public bool isSwitching = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        weaponShooting = GetComponent<WeaponShooting>();
    }

    void Start()
    {
        InitVariables();    
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        // avoid press switch when player is switching
        if (isSwitching)
        {
            _input.weapon = 0;
            return;
        }

        if (_input.weapon == 1 && (int)currentWeaponSO.weaponType != 1)
        {
            UnequipWeapon();
            EquipWeapon(weapons[0]);
        }
        else if (_input.weapon == 2 && (int)currentWeaponSO.weaponType != 0)
        {
            UnequipWeapon();
            EquipWeapon(weapons[1]);
        }

        _input.weapon = 0;
    }

    private void EquipWeapon(WeaponSO weaponSO)
    {
        
        //Set new weapon data
        currentWeaponSO = weaponSO;
        anim.SetInteger("WeaponType", (int)weaponSO.weaponType);
    }

    private void UnequipWeapon()
    {
        isSwitching = true;
        weaponShooting.canShoot = false;
        anim.SetTrigger("Unequip");
    }

    private void InitVariables()
    {
        EquipWeapon(weapons[1]);
    }
}
