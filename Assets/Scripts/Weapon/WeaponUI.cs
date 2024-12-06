using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI magazineSizeText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;

    public void UpdateInfo(Sprite weaponIcon, int magazineSize, int totalAmmo)
    {
        icon.sprite = weaponIcon;
        magazineSizeText.text = magazineSize.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }
}
