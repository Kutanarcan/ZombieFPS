using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float attackAngle;
    [SerializeField]
    AudioClip[] breathSounds;
    [SerializeField]
    AudioClip attackSound;
    [SerializeField]
    AudioClip deadSound;

    public EnemyDamageController EnemyDamageController { get; private set; }

    NavMeshAgent agent;
    EnemyAnimator enemyAnimator;
    Collider[] hitCollider;
    PlayerHealth playerHealth;
    GameObject target;
    AudioSource audioSource;
    WaitForSeconds breathSoundSeconds;

    bool isAttacking;
    float speed;
    float angularSpeed;

    const float BREATH_WAIT_SECONDS = 2f;

    void Awake()
    {
        breathSoundSeconds = new WaitForSeconds(BREATH_WAIT_SECONDS);

        enemyAnimator = GetComponent<EnemyAnimator>();
        agent = GetComponent<NavMeshAgent>();
        EnemyDamageController = GetComponent<EnemyDamageController>();
        hitCollider = GetComponentsInChildren<Collider>();
        target = GameObject.Find("Player");
        playerHealth = target.GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
        speed = agent.speed;
        angularSpeed = agent.angularSpeed;
        StartCoroutine(BreathSoundCoroutine());
    }

    IEnumerator BreathSoundCoroutine()
    {
        int rand = 0;

        while (true)
        {
            audioSource.PlayOneShot(breathSounds[rand]);
            yield return breathSoundSeconds;
            rand = Random.Range(0, breathSounds.Length);
        }
    }

    void Update()
    {
        if (EnemyDamageController.IsDead)
            return;

        agent.destination = target.transform.position;
        HandleAttack();
        EnemyBehindPlayerIndicator();
    }

    public void EnemyBehindPlayerIndicator()
    {
        float distanceWithTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceWithTarget < 10f)
        {
            Vector3 a, b;
            a = transform.forward;
            b = target.transform.forward;
            a = a.normalized;
            b = b.normalized;

            float backPivot = a.x * b.x + a.y * b.y + a.z * b.z;

            if (backPivot > -0.5f)
                PlayerController.Instance.PlayerWarn.ShowWarnText();
            else
                PlayerController.Instance.PlayerWarn.HideWarnText();
        }
        else
            PlayerController.Instance.PlayerWarn.HideWarnText();

    }

    public void Dead()
    {
        audioSource.PlayOneShot(deadSound);

        EnemyDamageController.IsDead = true;
        agent.updatePosition = false;

        for (int i = 0; i < hitCollider.Length; i++)
        {
            hitCollider[i].enabled = false;
        }

        enemyAnimator.CrossFadeDeath();
        Destroy(gameObject, 5f);
    }

    void HandleAttack()
    {
        if (EnemyDamageController.IsDead || isAttacking)
            return;

        float distanceWithTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceWithTarget < 2.3f)
        {
            Vector3 direction = target.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= attackAngle)
                Attack();
        }

        void Attack()
        {
            audioSource.PlayOneShot(attackSound);

            playerHealth.TakeDamage(damage);

            agent.speed = 0;
            agent.angularSpeed = 0;

            isAttacking = true;
            enemyAnimator.SetAttackTrigger();

            Invoke("ResetAttackTrigger", 1f);
        }
    }

    public void ResetAttackTrigger()
    {
        isAttacking = false;

        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }

}
