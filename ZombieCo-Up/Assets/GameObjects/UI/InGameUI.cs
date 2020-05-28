using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    GameObject playerStatsPanel;
    [SerializeField]
    TextMeshProUGUI ammoText;
    [SerializeField]
    TextMeshProUGUI weaponText;
    [SerializeField]
    TextMeshProUGUI playerHealthText;
    [SerializeField]
    TextMeshProUGUI moneyText;
    [SerializeField]
    TextMeshProUGUI killCountText;

    [Header("Shop Menu")]
    [SerializeField]
    GameObject shopMenu;

    bool activenessOfBuyPanel = false;

    void OnEnable()
    {
        UIEvents.OnPlayerStatChanged += UpdatePlayerStats;
        UIEvents.OnPlayerHealthChanged += UpdatePlayerHealth;
        UIEvents.OnMoneyChanged += UpdateMoney;
        UIEvents.OnKillCountChanged += UIEvents_OnKillCountChanged;
    }

    private void UIEvents_OnKillCountChanged(int killCount)
    {
        killCountText.text = $"Enemy Kill Count : {killCount.ToString()}";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            HandleBuyPanelActiveness();
        }
    }

    void HandleBuyPanelActiveness()
    {
        activenessOfBuyPanel = !activenessOfBuyPanel;

        UIEvents.OnBuyMenuActivenessChangedFunc(activenessOfBuyPanel);

        Time.timeScale = activenessOfBuyPanel ? 0f : 1f;

        Cursor.lockState = activenessOfBuyPanel ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = activenessOfBuyPanel ? true : false;

        shopMenu.SetActive(activenessOfBuyPanel);
    }

    public void UpdatePlayerStats(int ammo, int fullAmmo, string weaponName)
    {
        ammoText.text = $"{ammo.ToString()} / {fullAmmo.ToString()}";
        weaponText.text = weaponName;
    }

    public void UpdatePlayerHealth(float health)
    {
        playerHealthText.text = $"Health : {health.ToString("f0")}";
    }

    public void UpdateMoney(int amount)
    {
        moneyText.text = $"Money : {amount.ToString()}";
    }

    void OnDisable()
    {
        UIEvents.OnPlayerStatChanged -= UpdatePlayerStats;
        UIEvents.OnPlayerHealthChanged -= UpdatePlayerHealth;
        UIEvents.OnMoneyChanged -= UpdateMoney;
    }
}