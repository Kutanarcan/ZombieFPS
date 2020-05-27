using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    GameObject optionMenuPanel;
    
    public void LoadGame()
    {
        SceneController.Instance.LoadNextScene();
    }

    public void OpenOptionMenu()
    {
        mainMenuPanel.SetActive(false);
        optionMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
