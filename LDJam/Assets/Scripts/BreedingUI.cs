using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BreedingUI : MonoBehaviour
{
    [SerializeField] private Transform _baseParent;
    [SerializeField] private CreatureUIButton _penButton1;
    [SerializeField] private CreatureUIButton _penButton2;
    [SerializeField] private CreatureUIButton _creatureButtonPrefab;
    [SerializeField] private Button _closeButton;

    private Base _base;
    private BreedingPen _breedingPen;
    
    public Action OnMenuClose;

    private void Awake()
    {
        _closeButton.onClick.AddListener(_onClosePressed);
    }

    public void Init(BreedingPen pen)
    {
        _breedingPen = pen;

        if (pen.CreaturesInPen[0] == null)
        {
            _penButton1.Init(CreatureButtonState.InBreedingPen, null);
        }
        else
        {
            _penButton1.Init(CreatureButtonState.InBreedingPen, pen.CreaturesInPen[0]);
        }
        
        if (pen.CreaturesInPen[1] == null)
        {
            _penButton2.Init(CreatureButtonState.InBreedingPen, null);
        }
        else
        {
            _penButton2.Init(CreatureButtonState.InBreedingPen, pen.CreaturesInPen[1]);
        }

        _penButton1.OnButtonPressed += _onPenButtonPressed;
        _penButton2.OnButtonPressed += _onPenButtonPressed;

        _createBaseButtons();
    }

    private void _createBaseButtons()
    {
        foreach (var creature in _base.CreaturesInBase)
        {
            var button = Instantiate(_creatureButtonPrefab, _baseParent);
            button.Init(CreatureButtonState.InBase, creature);

            button.OnButtonPressed += _onBaseButtonPressed;
        }
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(_onClosePressed);
        
        _penButton1.OnButtonPressed -= _onPenButtonPressed;
        _penButton2.OnButtonPressed -= _onPenButtonPressed;

        for (int i = 0; i < _baseParent.childCount; i++)
        {
            _baseParent.GetChild(i).GetComponent<CreatureUIButton>().OnButtonPressed -= _onBaseButtonPressed;
        }
    }

    private void _onPenButtonPressed(CreatureUIButton button)
    {
        if (button.AttachedCreature != null)
        {
            _breedingPen.TransferCreatureToBase(button.AttachedCreature);
            
            var newBaseButton = Instantiate(_creatureButtonPrefab, _baseParent);
            newBaseButton.Init(CreatureButtonState.InBase, button.AttachedCreature);
            
            button.SetAttachedCreature(null);
        }
    }

    private void _onBaseButtonPressed(CreatureUIButton button)
    {
        if (_penButton1.AttachedCreature == null)
        {
            _base.TransferCreatureToBreedingPen(button.AttachedCreature, _breedingPen);
            _penButton1.SetAttachedCreature(button.AttachedCreature);
            Destroy(button.gameObject);
        }
        else if (_penButton2.AttachedCreature == null)
        {
            _base.TransferCreatureToBreedingPen(button.AttachedCreature, _breedingPen);
            _penButton2.SetAttachedCreature(button.AttachedCreature);
            Destroy(button.gameObject);
        }
        else
        {
            _base.TransferCreatureToBreedingPen(button.AttachedCreature, _breedingPen);
            _breedingPen.TransferCreatureToBase(_penButton1.AttachedCreature);
            
            var newBaseButton = Instantiate(_creatureButtonPrefab, _baseParent);
            newBaseButton.Init(CreatureButtonState.InBase, _penButton1.AttachedCreature);
            
            _penButton1.SetAttachedCreature(button.AttachedCreature);
            Destroy(button.gameObject);
        }
    }

    private void _onClosePressed()
    {
        OnMenuClose?.Invoke();
        
        Destroy(gameObject);
    }
}
