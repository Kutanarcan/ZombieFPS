using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        UIEvents.OnPlayerHealthChangedFunc(health.health);
    }

    public void TakeDamage(float damageAmount)
    {
        health.health -= damageAmount;
        UIEvents.OnPlayerHealthChangedFunc(health.health);
        PlayerEvents.OnPlayerTakeDamageFunc();

        if (health.health <= 0f)
        {
            Death();
        }
    }

    private void Death()
    {
        PlayerEvents.OnPlayerDeadFunc();
    }
}
