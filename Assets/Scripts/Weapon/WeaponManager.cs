using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    //[SerializeField] private WeaponSO[] weapons;

    [Tooltip("Arm location for holding weapon")]
    public Transform weaponHolderR;

    //References
    private StarterAssetsInputs _input;
    private WeaponShooting weaponShooting;
    private Inventory inventory;
    private PlayerHUD hud;

    //
    [HideInInspector] public GameObject currentWeaponObject;
    [HideInInspector] public WeaponSO currentWeaponSO;
    [HideInInspector] public Transform muzzleEffectPosition;

    // Check if in switching sate
    public bool isSwitching = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null) Instance = this;

        _input = GetComponent<StarterAssetsInputs>();
        inventory = GetComponent<Inventory>();
        weaponShooting = GetComponent<WeaponShooting>();
        hud = GetComponent<PlayerHUD>();
    }

    void Start()
    {
        //Set default weapon when starting
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

        if (_input.weapon == 1 && (int)currentWeaponSO.weaponType != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.weapons[0]);
        }
        else if (_input.weapon == 2 && (int)currentWeaponSO.weaponType != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.weapons[1]);
        }

        _input.weapon = 0;
    }

    private void EquipWeapon(WeaponSO weaponSO)
    {
        //Set new weapon data
        currentWeaponSO = weaponSO;
        AnimationEventManager.Instance.SetWeaponType(weaponSO);
        hud.UpdateWeaponUI(weaponSO);
    }

    private void UnequipWeapon()
    {
        isSwitching = true;
        weaponShooting.canShoot = false;
        AnimationEventManager.Instance.SetTriggerUnequip();
    }

    private void InitVariables()
    {
        EquipWeapon(inventory.weapons[1]);
    }
}
