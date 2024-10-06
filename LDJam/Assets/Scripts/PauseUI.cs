using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseUI : MonoBehaviour
{
    public Action OnMenuClose;

    private void _onCloseClicked()
    {
        OnMenuClose?.Invoke();

        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _onCloseClicked();
        }
    }

    public void ResumeGame()
    {
        _onCloseClicked();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
