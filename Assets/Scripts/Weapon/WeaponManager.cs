using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon weapon;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetWeaponAnim(WeaponType.Rifle);
    }

    private void SetWeaponAnim(WeaponType weaponType)
    {
        anim.SetInteger("WeaponType", (int)weaponType);
    }
}
