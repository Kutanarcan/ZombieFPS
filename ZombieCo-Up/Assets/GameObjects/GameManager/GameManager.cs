using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<float> enemyBehindChecker = new List<float>();

    float backAmount = 0;

    public int KillCount { get; set; }

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        PlayerEvents.OnPlayerDead += OnGameOver;
        KillCount = 0;
    }

    public void OnGameOver()
    {
        SceneController.Instance.ReLoadScene();

        if (KillCount > PlayerPrefs.GetInt("EnemyKillCount", 0))
            PlayerPrefs.SetInt("EnemyKillCount", KillCount);
    }

    void OnDisable()
    {
        PlayerEvents.OnPlayerDead -= OnGameOver;
    }

    void LateUpdate()
    {
        HandleEnemiesBehind();
    }

    void HandleEnemiesBehind()
    {

        backAmount = (enemyBehindChecker.Count > 0) ? 0 : -1f;

        for (int i = 0; i < enemyBehindChecker.Count; i++)
        {
            backAmount += enemyBehindChecker[i];
        }

        backAmount = Mathf.Clamp(backAmount, -1f, 1f);

        if (backAmount > -0.5f)
            PlayerController.Instance.PlayerWarn.ShowWarnText();
        else
            PlayerController.Instance.PlayerWarn.HideWarnText();
    }

    public void AddEnemyChecker(float backAmount)
    {
        enemyBehindChecker.Add(backAmount);
    }

    public void RemoveEnemyChecker(float backAmount)
    {
        if (enemyBehindChecker.Contains(backAmount))
            enemyBehindChecker.Remove(backAmount);
    }
}
