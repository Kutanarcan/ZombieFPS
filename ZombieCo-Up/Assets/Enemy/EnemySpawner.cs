using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float spawnDuration = 5f;
    [SerializeField]
    GameObject zombiePrefab;

    List<Transform> spawnPoints;
    float spawnTimer;
    float spawnRate;
    int randSpawnIndex = 0;

    void Awake()
    {
        spawnTimer = spawnDuration;
        spawnRate = spawnDuration;

        spawnPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
    }

    void Update()
    {
        if (Time.time > spawnTimer)
        {
            randSpawnIndex = Random.Range(0, spawnPoints.Count);

            if (spawnRate > 2.1f)
                spawnRate -= 0.1f;

            spawnTimer = Time.time + spawnRate;

            Instantiate(zombiePrefab, spawnPoints[randSpawnIndex].position, spawnPoints[randSpawnIndex].rotation);
        }
    }
}
