using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureUIButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    
    private Button _button;

    public Action<CreatureUIButton> OnButtonPressed;
    public Creature AttachedCreature { get; private set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(_onButtonPressed);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(_onButtonPressed);
    }

    public void Init(CreatureButtonState buttonState, Creature creature)
    {
        AttachedCreature = creature;

        if (buttonState == CreatureButtonState.InBreedingPen)
        {
            if (AttachedCreature != null)
            {
                _buttonText.text = AttachedCreature.CurrentName;
            }
            else
            {
                _buttonText.text = "Empty";
            }
        }
        else
        {
            _buttonText.text = AttachedCreature.CurrentName;
        }
    }

    public void SetAttachedCreature(Creature creature)
    {
        AttachedCreature = creature;
        
        if (AttachedCreature != null)
        {
            _buttonText.text = AttachedCreature.CurrentName;
        }
        else
        {
            _buttonText.text = "Empty";
        }
    }

    private void _onButtonPressed()
    {
        OnButtonPressed?.Invoke(this);
    }
}

public enum CreatureButtonState
{
    InBase,
    InBreedingPen
}