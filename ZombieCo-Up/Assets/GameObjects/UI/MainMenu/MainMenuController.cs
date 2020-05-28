using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    TextMeshProUGUI killCount;

    void Awake()
    {
        killCount.text = $"Best Enemy Kill Count : {PlayerPrefs.GetInt("EnemyKillCount", 0).ToString()}";
    }

    public void LoadGame()
    {
        SceneController.Instance.LoadNextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
