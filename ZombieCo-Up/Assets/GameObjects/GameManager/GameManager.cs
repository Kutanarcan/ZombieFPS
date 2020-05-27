using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void OnEnable()
    {
        PlayerEvents.OnPlayerDead += OnGameOver;
    }

    public void OnGameOver()
    {
        SceneController.Instance.ReLoadScene();
    }

    void OnDisable()
    {
        PlayerEvents.OnPlayerDead -= OnGameOver;
    }
}
