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
    float backPivotAmount;

    const float BREATH_WAIT_SECONDS = 4f;

    void Awake()
    {
        breathSoundSeconds = new WaitForSeconds(BREATH_WAIT_SECONDS);

        enemyAnimator = GetComponent<EnemyAnimator>();
        agent = GetComponent<NavMeshAgent>();
        EnemyDamageController = GetComponent<EnemyDamageController>();
        hitCollider = GetComponentsInChildren<Collider>();
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

    void OnEnable()
    {        
        
        PlayerEvents.OnPlayerDead += PlayerEvents_OnPlayerDead;
        target = GameObject.Find("Player");
        playerHealth = target.GetComponent<PlayerHealth>();
    }

    private void PlayerEvents_OnPlayerDead()
    {
        ObjectPooler.Instance.ReturnToPool(gameObject.transform.name, gameObject);
    }

    public void EnemyBehindPlayerIndicator()
    {
        if (EnemyDamageController.IsDead)
            GameManager.Instance.RemoveEnemyChecker(backPivotAmount);
        else
        {
            float distanceWithTarget = Vector3.Distance(target.transform.position, transform.position);

            if (distanceWithTarget < 15f && distanceWithTarget > 2f)
            {
                Vector3 a, b;
                a = transform.forward;
                b = target.transform.forward;
                a = a.normalized;
                b = b.normalized;

                GameManager.Instance.RemoveEnemyChecker(backPivotAmount);

                backPivotAmount = a.x * b.x + a.y * b.y + a.z * b.z;
                if (backPivotAmount > -0.5f)
                    GameManager.Instance.AddEnemyChecker(backPivotAmount);
            }
            else
                GameManager.Instance.RemoveEnemyChecker(backPivotAmount);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RemoveEnemyChecker(backPivotAmount);

        PlayerEvents.OnPlayerDead -= PlayerEvents_OnPlayerDead;
    }

    public void Dead()
    {
        GameManager.Instance.KillCount++;
        UIEvents.OnKillCountChangedFunc(GameManager.Instance.KillCount);

        audioSource.PlayOneShot(deadSound);

        EnemyDamageController.IsDead = true;
        agent.updatePosition = false;

        for (int i = 0; i < hitCollider.Length; i++)
        {
            hitCollider[i].enabled = false;
        }

        enemyAnimator.CrossFadeDeath();
        ObjectPooler.Instance.ReturnToPool(gameObject.transform.name, gameObject, 5f);
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
