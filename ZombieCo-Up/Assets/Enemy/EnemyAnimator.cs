using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator anim;

    static int paramDeath = Animator.StringToHash("Death");
    static int paramWalk = Animator.StringToHash("Walk");
    static int paramAttack = Animator.StringToHash("Attack");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void CrossFadeDeath()
    {
        anim.CrossFadeInFixedTime(paramDeath, 0.1f);
    }

    public void CrossFadeWalk()
    {
        anim.CrossFadeInFixedTime(paramWalk, 0.1f);
    }

    public void SetAttackTrigger()
    {
        anim.SetTrigger(paramAttack);
    }
}
