using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public static UIBase Instance { get; private set; }

    public InGameUI InGameUI { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        InGameUI = GetComponentInChildren<InGameUI>();
    }
}

public static class UIEvents
{
    public static event System.Action<int, int, string> OnPlayerStatChanged;
    public static event System.Action<float> OnPlayerHealthChanged;
    public static event System.Action<int> OnMoneyChanged;
    public static event System.Action<bool> OnBuyMenuActivenessChanged;
    public static event System.Action<int> OnKillCountChanged;

    public static void OnPlayerStatChangedFunc(int ammo, int fullAmmo, string weaponName)
    {
        OnPlayerStatChanged?.Invoke(ammo, fullAmmo, weaponName);
    }

    public static void OnPlayerHealthChangedFunc(float health)
    {
        OnPlayerHealthChanged?.Invoke(health);
    }

    public static void OnMoneyChangedFunc(int moneyAmount)
    {
        OnMoneyChanged?.Invoke(moneyAmount);
    }

    public static void OnBuyMenuActivenessChangedFunc(bool activeness)
    {
        OnBuyMenuActivenessChanged?.Invoke(activeness);
    }

    public static void OnKillCountChangedFunc(int newKillCount)
    {
        OnKillCountChanged?.Invoke(newKillCount);
    }
}