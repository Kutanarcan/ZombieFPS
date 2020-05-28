using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPartType
{
    Head,
    Body,
    Arm,
    Leg
}

public class BodyPart : MonoBehaviour
{
    [SerializeField]
    BodyPartType bodyPartType;
    [SerializeField]
    GameObject bloodEffect;
    [SerializeField]
    AudioClip[] bloodSounds;
    [SerializeField]
    AudioClip lastBloodSoundToPlay;

    public BodyPartType BodyPartType => bodyPartType;

    Enemy enemy;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        enemy = GetComponentInParent<Enemy>();
    }

    public void TakeDamage(float damageAmount)
    {
        enemy.EnemyDamageController.TakeDamage(damageAmount);
        enemy.EnemyDamageController.CurrentDamageType = bodyPartType;
    }

    public void BloodEffect(Vector3 position, Quaternion rotation)
    {
        int rand = Random.Range(0, bloodSounds.Length);

        if (!enemy.EnemyDamageController.IsDead)
            audioSource.PlayOneShot(bloodSounds[rand]);
        else
            audioSource.PlayOneShot(lastBloodSoundToPlay);

        GameObject tmp = ObjectPooler.Instance.SpawnPoolObject(bloodEffect.name, position, rotation);
        ObjectPooler.Instance.ReturnToPool(tmp.name, tmp, 1f);
    }
}
