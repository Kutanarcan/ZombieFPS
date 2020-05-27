using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopMenuController : MonoBehaviour
{
    [SerializeField]
    ShopMenuWeaponHolder shopMenuWeaponHolderPrefab;
    [SerializeField]
    GameObject weaponBuyPanel;

    List<WeaponBase> weaponBases;

    void OnEnable()
    {
        InitializeWeaponShopPanel();
        Cursor.lockState = CursorLockMode.None;
    }

    void InitializeWeaponShopPanel()
    {
        weaponBases = PlayerController.Instance.WeaponManager.weaponBases;

        for (int i = 0; i < weaponBases.Count; i++)
        {
            WeaponBase weaponBase = weaponBases[i];
            WeaponShopInfo weaponShopInfo = weaponBase.WeaponShopInfo;
            ShopMenuWeaponHolder weaponUIHolder = Instantiate(shopMenuWeaponHolderPrefab, weaponBuyPanel.transform);

            weaponShopInfo.Initialize(weaponBase);
            weaponUIHolder.WeaponToBuyBTN.interactable = !weaponBase.IsActive;

            weaponUIHolder.WeaponName.text = weaponShopInfo.Weapon.ToString();
            weaponUIHolder.WeaponCostText.text = weaponShopInfo.WeaponCost.ToString();
            weaponUIHolder.WeaponToBuyBTN.GetComponent<Image>().sprite = weaponShopInfo.WeaponSprite;
            weaponUIHolder.WeaponToBuyBTN.onClick.AddListener(() =>
            {
                if (CashSystem.Instance.CurrentCash >= weaponShopInfo.WeaponCost)
                {
                    CashSystem.Instance.CurrentCash -= weaponShopInfo.WeaponCost;
                    UIEvents.OnMoneyChangedFunc(CashSystem.Instance.CurrentCash);

                    weaponShopInfo.BuyWeapon?.Invoke();
                    UpdateWeaponBuy(weaponBase, weaponShopInfo, weaponUIHolder);
                }
            });

            int fullAmmoCost = (weaponShopInfo.AmmoCostPerBullet * weaponBase.BulletsToBuy);

            weaponUIHolder.AmmoCostText.text = fullAmmoCost.ToString();

            weaponUIHolder.WeaponAmmoToBuyBTN.interactable = weaponBase.IsActive;

            if (weaponBase.BulletsToBuy == 0) weaponUIHolder.WeaponAmmoToBuyBTN.interactable = false;
            else weaponUIHolder.WeaponAmmoToBuyBTN.interactable = true;

            weaponUIHolder.WeaponAmmoToBuyBTN.onClick.AddListener(() =>
            {
                if (CashSystem.Instance.CurrentCash >= fullAmmoCost)
                {
                    CashSystem.Instance.CurrentCash -= fullAmmoCost;
                    UIEvents.OnMoneyChangedFunc(CashSystem.Instance.CurrentCash);

                    weaponShopInfo.BuyAmmo?.Invoke();
                    UpdateAmmoBuy(weaponBase, weaponShopInfo, weaponUIHolder);
                }
            });
        }
    }

    void OnDisable()
    {
        ClearBuyMenu();
    }

    void UpdateWeaponBuy(WeaponBase weaponBase, WeaponShopInfo weaponShopInfo, ShopMenuWeaponHolder weaponUIHolder)
    {
        weaponUIHolder.WeaponToBuyBTN.interactable = !weaponBase.IsActive;

        if (weaponBase.BulletsToBuy == 0) weaponUIHolder.WeaponAmmoToBuyBTN.interactable = false;

        weaponUIHolder.AmmoCostText.text = (weaponShopInfo.AmmoCostPerBullet * weaponBase.BulletsToBuy).ToString();
    }

    void UpdateAmmoBuy(WeaponBase weaponBase, WeaponShopInfo weaponShopInfo, ShopMenuWeaponHolder weaponUIHolder)
    {
        weaponUIHolder.AmmoCostText.text = (weaponShopInfo.AmmoCostPerBullet * weaponBase.BulletsToBuy).ToString();
        if (weaponBase.BulletsToBuy == 0) weaponUIHolder.WeaponAmmoToBuyBTN.interactable = false;
        else weaponUIHolder.WeaponAmmoToBuyBTN.interactable = true;
    }

    void ClearBuyMenu()
    {
        foreach (Transform child in weaponBuyPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
