using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private ParticleSystem muzzleFlash;

    public void Shoot(WeaponSO weaponSO)
    {
        RaycastHit hit; //Variable for storing the information of WHAT WE HIT
        muzzleFlash.Play();

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

            GameObject impact = Instantiate(weaponSO.hitEffect, hit.point, Quaternion.LookRotation(hit.normal), parent.transform);
            Destroy(impact, 1f);
        }
    }
}
