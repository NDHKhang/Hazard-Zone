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

    //Weapon
    [HideInInspector] public GameObject currentWeaponObject;
    [HideInInspector] public int currentWeaponIndex;
    [HideInInspector] public Transform muzzleEffectPosition;

    //Default init weapon (AK & Pistol)
    [SerializeField] private WeaponSO primaryWeapon;
    [SerializeField] private WeaponSO secondaryWeapon;

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

        if (_input.weapon == 1 && (int)inventory.GetWeapon(currentWeaponIndex).weaponStyle != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetWeapon(0));
        }
        else if (_input.weapon == 2 && (int)inventory.GetWeapon(currentWeaponIndex).weaponStyle != 1)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetWeapon(1));
        }

        _input.weapon = 0;
    }

    private void EquipWeapon(WeaponSO weapon)
    {
        //Set new weapon data
        currentWeaponIndex = (int)weapon.weaponStyle;
        AnimationEventManager.Instance.SetWeaponType(weapon);
        hud.UpdateWeaponUI(weapon);
    }

    private void UnequipWeapon()
    {
        isSwitching = true;
        AnimationEventManager.Instance.SetTriggerUnequip();
    }
        
    private void InitVariables()
    {
        inventory.AddWeapon(primaryWeapon);
        inventory.AddWeapon(secondaryWeapon);
        EquipWeapon(inventory.weapons[(int)secondaryWeapon.weaponStyle]);
    }
}
