using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderShotgun : TubeMagazineWeapon
{
    public void OnPump()
    {
        audioSource.PlayOneShot(boltSound);
    }

    public override void OnBoltForwarded()
    {
        Invoke("ResetIsReloading", 0.1f);
    }
}
