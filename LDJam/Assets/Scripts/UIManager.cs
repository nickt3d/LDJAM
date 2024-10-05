using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CreatureNameUI _creatureNameUI;
    
    public static UIManager Instance { get; private set; }

    private StarterAssetsInputs _starterAssetsInputs;
    private CreatureNameUI _creatureNameUIInstance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    private void Start()
    {
        _starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    public void OpenCreatureNameUI(Creature creature)
    {
        _starterAssetsInputs.SetCursorState(false);
        _starterAssetsInputs.SetCursorInputForLook(false);
        
        _creatureNameUIInstance = Instantiate(_creatureNameUI, transform);
        _creatureNameUIInstance.Init(creature);

        _creatureNameUIInstance.OnMenuClose += _onCreatureNameUIClose;
    }

    private void _onCreatureNameUIClose()
    {
        _creatureNameUIInstance.OnMenuClose -= _onCreatureNameUIClose;
        
        _starterAssetsInputs.SetCursorState(true);
        _starterAssetsInputs.SetCursorInputForLook(true);
        _creatureNameUIInstance = null;
    }

    public bool IsMenuOpen()
    {
        //TODO: Add the other ui menu into this when it exists
        return _creatureNameUIInstance != null;
    }
}
