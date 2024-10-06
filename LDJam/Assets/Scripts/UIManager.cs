using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CreatureNameUI _creatureNameUI;
    [SerializeField] private BreedingUI _breedingUI;
    
    public static UIManager Instance { get; private set; }

    private StarterAssetsInputs _starterAssetsInputs;
    private CreatureNameUI _creatureNameUIInstance;
    private BreedingUI _breedingUIInstance;
    
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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_creatureNameUIInstance != null)
            {
                Destroy(_creatureNameUIInstance.gameObject);
                _onCreatureNameUIClose();
            }

            if (_breedingUIInstance != null)
            {
                Destroy(_breedingUIInstance.gameObject);
                _onBreedingUIClose();
            }
        }
    }

    public void OpenCreatureNameUI(Creature creature)
    {
        if (_creatureNameUIInstance == null)
        {
            _starterAssetsInputs.SetCursorState(false);
            _starterAssetsInputs.SetCursorInputForLook(false);

            _creatureNameUIInstance = Instantiate(_creatureNameUI, transform);
            _creatureNameUIInstance.Init(creature);

            _creatureNameUIInstance.OnMenuClose += _onCreatureNameUIClose;
        }
    }

    public void OpenBreedingUI(BreedingPen pen)
    {
        if (_breedingUIInstance == null)
        {
            _starterAssetsInputs.SetCursorState(false);
            _starterAssetsInputs.SetCursorInputForLook(false);

            _breedingUIInstance = Instantiate(_breedingUI, transform);
            _breedingUIInstance.Init(pen);

            _breedingUIInstance.OnMenuClose += _onBreedingUIClose;
        }
    }

    private void _onCreatureNameUIClose()
    {
        _creatureNameUIInstance.OnMenuClose -= _onCreatureNameUIClose;
        
        _starterAssetsInputs.SetCursorState(true);
        _starterAssetsInputs.SetCursorInputForLook(true);
        _creatureNameUIInstance = null;
    }

    private void _onBreedingUIClose()
    {
        _breedingUIInstance.OnMenuClose -= _onBreedingUIClose;
        
        _starterAssetsInputs.SetCursorState(true);
        _starterAssetsInputs.SetCursorInputForLook(true);
        _breedingUIInstance = null;
    }

    public bool IsMenuOpen()
    {
        return _creatureNameUIInstance != null || _breedingUIInstance != null;
    }
}
