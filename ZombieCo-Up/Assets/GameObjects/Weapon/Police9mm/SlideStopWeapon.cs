using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideStopWeapon : WeaponBase
{

    protected override void PlayFireAnimation()
    {
        if (bulletsInClip > 1)
            weaponAnimation.CrossFadeShoot();
        else
            weaponAnimation.CrossFadeLastShot();
    }
}
