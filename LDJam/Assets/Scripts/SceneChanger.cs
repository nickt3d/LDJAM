using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    [SerializeField] string sceneName;
    [SerializeField] TransitionSettings transition;
    [SerializeField] float startDelay;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void LoadGameScene()
    {
        TransitionManager.Instance().Transition(sceneName, transition, startDelay);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadGameScene();
        }
    }
}
