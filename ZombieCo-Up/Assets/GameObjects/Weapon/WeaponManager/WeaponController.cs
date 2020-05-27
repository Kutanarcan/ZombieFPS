using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    KeyCode weaponChangeKey = KeyCode.Alpha1;

    int index = 0;
    WeaponManager weaponManager;
    PlayerLook playerLook;

    private void Awake()
    {
        weaponManager = GetComponentInParent<WeaponManager>();
        playerLook = GetComponentInParent<PlayerLook>();
    }

    private void OnEnable()
    {
        FindFirstAvaibleWeapon();
        SelectWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(weaponChangeKey))
        {
            SelectWeapon();
        }
    }

    WeaponBase weaponBase;

    void SelectWeapon()
    {
        int tmpIndex = index;
        bool hasActive = false;

        while (!hasActive)
        {
            weaponBase = transform.GetChild(tmpIndex).GetComponent<WeaponBase>();

            if (weaponBase.IsActive)
            {
                index = tmpIndex;
                hasActive = true;
            }
            tmpIndex++;
            if (tmpIndex >= transform.childCount)
                tmpIndex = 0;

            if (tmpIndex == index) return;    
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        weaponBase.gameObject.SetActive(true);
        weaponBase.UpdateUIStats();
        weaponManager.CurrentWeapon = weaponBase.Weapon;

        playerLook.RecoilRecoverAmount = 20f;

        index++;

        if (index >= transform.childCount)
            index = 0;
    }

    void FindFirstAvaibleWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            WeaponBase weaponBase = transform.GetChild(i).GetComponent<WeaponBase>();

            if (weaponBase.IsActive)
            {
                index = i;
                break;
            }
        }
    }
}
