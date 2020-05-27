using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    Police9mm = 0,
    PortableMagnum = 10,
    Compact9mm = 20,
    UMP45 = 30,
    StovRifle = 40,
    DefenderShotgun = 50
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    GameObject primaryWeapon;
    [SerializeField]
    GameObject secondaryWeapon;

    const KeyCode FIRST_WEAPON = KeyCode.Alpha1;
    const KeyCode SECONDARY_WEAPON = KeyCode.Alpha2;

    public List<WeaponBase> weaponBases { get; private set; } = new List<WeaponBase>();
    public Weapon CurrentWeapon { get; set; }

    void Awake()
    {
        primaryWeapon.SetActive(true);
        InitializeWeaponList();
    }

    void InitializeWeaponList()
    {
        for (int i = 0; i < primaryWeapon.transform.childCount; i++)
        {
            weaponBases.Add(primaryWeapon.transform.GetChild(i).GetComponent<WeaponBase>());
        }

        for (int i = 0; i < secondaryWeapon.transform.childCount; i++)
        {
            weaponBases.Add(secondaryWeapon.transform.GetChild(i).GetComponent<WeaponBase>());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(FIRST_WEAPON))
        {
            secondaryWeapon.SetActive(false);
            primaryWeapon.SetActive(true);
        }

        if (Input.GetKeyDown(SECONDARY_WEAPON))
        {
            primaryWeapon.SetActive(false);
            secondaryWeapon.SetActive(true);
        }
    }
}
