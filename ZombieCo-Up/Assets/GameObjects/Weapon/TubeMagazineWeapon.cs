using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeMagazineWeapon : WeaponBase
{
    [Header("Tube Magazine Weapon Sound References")]
    [SerializeField]
    AudioClip ammoInsertSound;

    protected override void Reload()
    {
        if (isRealoding) return;

        isRealoding = true;

        if (bulletsInClip <= 0)
        {
            weaponAnimation.CrossFadeReloadStartEmpty();
        }
        else
            weaponAnimation.CrossFadeReloadStart();
    }

    protected override void FillAmmo()
    {
        bulletsLeft--;
        bulletsInClip++;

        UpdateUIStats();
    }

    public void HandleNextReload()
    {
        isRealoding = true;
        bool stopInserting = false;

        if (bulletsLeft <= 0)
        {
            stopInserting = true;
        }
        else if (bulletsInClip >= clipSize)
        {
            stopInserting = true;
        }

        if (stopInserting)
        {
            weaponAnimation.CrossFadeReloadEnd();
        }
        else
        {
            weaponAnimation.CrossFadeReloadInsert();
        }
    }

    public void OnAmmoInserted()
    {
        isRealoding = false;
        audioSource.PlayOneShot(ammoInsertSound);
        FillAmmo();
    }
}
