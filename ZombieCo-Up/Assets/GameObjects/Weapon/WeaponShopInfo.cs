using System;
using UnityEngine;

public class WeaponShopInfo : MonoBehaviour
{
    [SerializeField]
    Weapon weapon;
    [SerializeField]
    int weaponCost;
    [SerializeField]
    Sprite weaponSprite;
    [SerializeField]
    int ammoCostPerBullet;

    public Weapon Weapon => weapon;
    public int WeaponCost => weaponCost;
    public Sprite WeaponSprite => weaponSprite;
    public int AmmoCostPerBullet => ammoCostPerBullet;

    public Action BuyWeapon => BuyWeaponFunc;
    public Action BuyAmmo => BuyAmmoFunc;

    WeaponBase weaponBase;

    public void Initialize(WeaponBase weaponBase)
    {
        this.weaponBase = weaponBase;
    }

    void BuyWeaponFunc()
    {
        weaponBase.HandleWeaponBuy();
    }

    void BuyAmmoFunc()
    {
        weaponBase.HandleAmmoBuy();
    }
}
