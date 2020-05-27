using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    Animator anim;

    static int paramHit = Animator.StringToHash("PlayerHit");

    void Awake()
    {
        anim = GetComponent<Animator>();
        PlayerEvents.OnPlayerTakeDamage += PlayerEvents_OnPlayerTakeDamage;
    }

    private void PlayerEvents_OnPlayerTakeDamage()
    {
        anim.CrossFade(paramHit, 0.1f);
    }

    void OnDisable()
    {
        PlayerEvents.OnPlayerTakeDamage -= PlayerEvents_OnPlayerTakeDamage;
    }
}
