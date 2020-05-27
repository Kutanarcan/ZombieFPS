using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashSystem : MonoBehaviour
{
    public static CashSystem Instance { get; private set; }

    public int CurrentCash { get; set; }

    int initialCashAmount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        CurrentCash = initialCashAmount;
    }

    public void UpdateMoneyUI()
    {
        UIEvents.OnMoneyChangedFunc(CurrentCash);
    }
}
