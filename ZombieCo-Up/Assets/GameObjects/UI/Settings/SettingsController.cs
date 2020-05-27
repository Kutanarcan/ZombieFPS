using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    Slider mouseSensivity;

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadScene(0);
    }

    public void SetMouseSensivity(float amount)
    {
        PlayerController.Instance.PlayerLook.MouseSensitivity = amount;
    }
}
