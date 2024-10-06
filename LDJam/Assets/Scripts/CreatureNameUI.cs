using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputfield;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _closebutton;

    public Action OnMenuClose;
    
    private Creature _creature;
    
    private void Awake()
    {
        _confirmButton.onClick.AddListener(_onConfirmClicked);
        _closebutton.onClick.AddListener(_onCloseClicked);
    }

    public void Init(Creature creature)
    {
        _creature = creature;
        _nameText.text = "Name: " + creature.CurrentName;
    }

    private void OnDestroy()
    {
        _confirmButton.onClick.RemoveListener(_onConfirmClicked);
    }

    private void _onConfirmClicked()
    {
        _creature.SetName(_inputfield.text);
        _nameText.text = "Name: " + _inputfield.text;
        
        _onCloseClicked();
    }

    private void _onCloseClicked()
    {
        OnMenuClose?.Invoke();
        
        Destroy(gameObject);
    }
}
