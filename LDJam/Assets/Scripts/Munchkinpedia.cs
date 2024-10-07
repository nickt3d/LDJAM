using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class Munchkinpedia : MonoBehaviour
{
    public Action OnMenuClose;
    public List<MuchkinpediaDisplay> _buttonList;

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();

        foreach (var button in _buttonList)
        {
            if (!_player.TamedCreatures.Contains(button.munchkin))
            {
                button.GetComponent<Button>().interactable = false;
                button.GetComponentInChildren<TMP_Text>().color = Color.grey;
            }
            else
            {
                button.GetComponent<Button>().interactable = true;
                button.GetComponentInChildren<TMP_Text>().color = Color.white;
            }
        }
    }

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
