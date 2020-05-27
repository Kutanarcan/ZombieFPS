using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    SceneController sceneController;

    void Awake()
    {
        sceneController = GetComponentInParent<SceneController>();
    }

    public void OnFadeComplete()
    {
        sceneController.OnFadeComplete();
    }
}
