using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWarn : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI warnText;    

    public void ShowWarnText()
    {
        warnText.gameObject.SetActive(true);
    }

    public void HideWarnText()
    {
        warnText.gameObject.SetActive(false);
    }
}
