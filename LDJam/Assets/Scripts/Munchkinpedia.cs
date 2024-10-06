using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Munchkinpedia : MonoBehaviour
{
    public Action OnMenuClose;

    private void _onCloseClicked()
    {
        OnMenuClose?.Invoke();

        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _onCloseClicked();
        }    
    }
}
