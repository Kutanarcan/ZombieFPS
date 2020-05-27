using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    Animator animator;

    static int paramFadeOut = Animator.StringToHash("FadeOut");
    int levelToLoad;
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        animator = GetComponentInChildren<Animator>();
    }

    public void LoadNextScene()
    {
        animator.SetTrigger(paramFadeOut);
        levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void ReLoadScene()
    {
        animator.SetTrigger(paramFadeOut);
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(int sceneIndex)
    {
        animator.SetTrigger(paramFadeOut);
        levelToLoad = sceneIndex;
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
