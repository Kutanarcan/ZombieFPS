using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour, IShootable, ICashGiver
{
    [SerializeField]
    AudioClip[] headshotSounds;
    [SerializeField]
    int killReward;
    [SerializeField]
    int headShotBonusReward;

    Health health;
    public bool IsDead { get; set; }
    Enemy enemy;

    AudioSource audioSource;

    public BodyPartType CurrentDamageType { get; set; }
    public int KillReward => killReward;

    void Awake()
    {
        health = GetComponent<Health>();
        enemy = GetComponent<Enemy>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        health.health -= damage;

        if (health.health <= 0)
        {
            enemy.Dead();

            GiveCash(KillReward);

            if (CurrentDamageType == BodyPartType.Head)
            {
                int rand = Random.Range(0, headshotSounds.Length);
                audioSource.PlayOneShot(headshotSounds[rand]);
                GiveCash(headShotBonusReward);
            }
        }
    }

    public void GiveCash(int cashAmount)
    {
        CashSystem.Instance.CurrentCash += cashAmount;
        UIEvents.OnMoneyChangedFunc(CashSystem.Instance.CurrentCash);
    }
}
