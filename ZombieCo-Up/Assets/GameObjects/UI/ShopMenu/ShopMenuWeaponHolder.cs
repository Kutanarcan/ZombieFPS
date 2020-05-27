using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuWeaponHolder : MonoBehaviour
{
    [SerializeField]
    Button weaponToBuyBTN;
    [SerializeField]
    Text weaponName;
    [SerializeField]
    Text weaponCostText;

    [SerializeField]
    Button weaponAmmoToBuyBTN;
    [SerializeField]
    Text ammoName;
    [SerializeField]
    Text ammoCostText;

    public Button WeaponToBuyBTN => weaponToBuyBTN;
    public Text WeaponName => weaponName;
    public Text WeaponCostText => weaponCostText;
    public Button WeaponAmmoToBuyBTN => weaponAmmoToBuyBTN;
    public Text AmmoName => ammoName;
    public Text AmmoCostText => ammoCostText;
}
