using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera Camera;

    [SerializeField] WeaponSO weaponSO;

    [Tooltip("Determine how far the bullet go")]
    [SerializeField] private float range = 100f;

    private StarterAssetsInputs _input;

    [Header("References")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject parent;
    [SerializeField] private Animator animator;

    void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (!_input.shoot) return;
        muzzleFlash.Play();

        RaycastHit hit; //Variable for storing the information of WHAT WE HIT

        //Shooting
        //When hit the enemy, decrease the enemy's health
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, range))
        {
            Debug.Log("Hit" + hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponSO.damage);
            }

            GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal), parent.transform);
            Destroy(impact, 1f);
        }
    }
}
