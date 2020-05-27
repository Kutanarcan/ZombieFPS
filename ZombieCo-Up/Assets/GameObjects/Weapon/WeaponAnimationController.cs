using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    Animator anim;

    protected static int paramFire = Animator.StringToHash("Fire");
    protected static int paramLastShot = Animator.StringToHash("FireLast");
    protected static int paramReload = Animator.StringToHash("Reload");
    protected static int paramReloadStartEmpty = Animator.StringToHash("ReloadStartEmpty");
    protected static int paramReloadStart = Animator.StringToHash("ReloadStart");
    protected static int paramReloadInsert = Animator.StringToHash("ReloadInsert");
    protected static int paramReloadEnd = Animator.StringToHash("ReloadEnd");

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void CrossFadeShoot()
    {
        anim.CrossFadeInFixedTime(paramFire, 0.1f);
    }

    public void CrossFadeLastShot()
    {
        anim.CrossFadeInFixedTime(paramLastShot, 0.1f);
    }

    public void CrossFadeReload()
    {
        anim.CrossFadeInFixedTime(paramReload, 0.1f);
    }

    public void CrossFadeReloadInsert()
    {
        anim.CrossFadeInFixedTime(paramReloadInsert, 0.1f);
    }

    public void CrossFadeReloadEnd()
    {
        anim.CrossFadeInFixedTime(paramReloadEnd, 0.1f);
    }

    public void CrossFadeReloadStartEmpty()
    {
        anim.CrossFadeInFixedTime(paramReloadStartEmpty, 0.1f);
    }

    public void CrossFadeReloadStart()
    {
        anim.CrossFadeInFixedTime(paramReloadStart, 0.1f);
    }
}
