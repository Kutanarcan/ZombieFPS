using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    Slider mouseSensivity;

    private void Awake()
    {
        mouseSensivity.value = PlayerPrefs.GetFloat("MouseSensivity", 250f);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        PlayerEvents.OnPlayerDeadFunc();
        SceneController.Instance.LoadScene(0);
    }

    public void SetMouseSensivity(float amount)
    {
        PlayerController.Instance.PlayerLook.MouseSensitivity = amount;
        PlayerPrefs.SetFloat("MouseSensivity", amount);
    }
}
