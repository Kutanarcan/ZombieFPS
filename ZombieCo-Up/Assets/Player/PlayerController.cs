using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public WeaponManager WeaponManager { get; private set; }
    public PlayerLook PlayerLook { get; private set; }
    public PlayerWarn PlayerWarn { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerInput = GetComponent<PlayerInput>();
        WeaponManager = GetComponentInChildren<WeaponManager>();
        PlayerLook = GetComponentInChildren<PlayerLook>();
        PlayerWarn = GetComponentInChildren<PlayerWarn>();
    }
}

public static class PlayerEvents
{
    public static event System.Action OnPlayerTakeDamage;
    public static event System.Action OnPlayerDead;

    public static void OnPlayerTakeDamageFunc()
    {
        OnPlayerTakeDamage?.Invoke();
    }

    public static void OnPlayerDeadFunc()
    {
        OnPlayerDead?.Invoke();
    }
}
