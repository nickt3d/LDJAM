using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CreatureNameUI _creatureNameUI;
    [SerializeField] private BreedingUI _breedingUI;
    [SerializeField] private Munchkinpedia _munchkinpediaUI;
    
    public static UIManager Instance { get; private set; }

    private StarterAssetsInputs _starterAssetsInputs;
    private CreatureNameUI _creatureNameUIInstance;
    private BreedingUI _breedingUIInstance;
    private Munchkinpedia _munchkinpediaUIInstance;
    
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

            if (_munchkinpediaUI != null)
            {
                Destroy(_munchkinpediaUI.gameObject);            
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenMunchkinpediaUI();
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

    private void OpenMunchkinpediaUI()
    {
        if (_munchkinpediaUIInstance == null)
        {
            _starterAssetsInputs.SetCursorState(false);
            _starterAssetsInputs.SetCursorInputForLook(false);

            _munchkinpediaUIInstance = Instantiate(_munchkinpediaUI, transform);

            _munchkinpediaUI.OnMenuClose += _onMunchkinpediaUIClose;
        }
    }

    private void _onCreatureNameUIClose()
    {
        _creatureNameUIInstance.OnMenuClose -= _onCreatureNameUIClose;
        _creatureNameUIInstance = null;
        
        _onMenuClose();
    }

    private void _onBreedingUIClose()
    {
        _breedingUIInstance.OnMenuClose -= _onBreedingUIClose;
        _breedingUIInstance = null;
        
        _onMenuClose();
    }

    private void _onMunchkinpediaUIClose()
    {
        _munchkinpediaUIInstance.OnMenuClose -= _onMunchkinpediaUIClose;
        _munchkinpediaUIInstance = null;

        _onMenuClose();
    }

    private void _onMenuClose()
    {
        _starterAssetsInputs.SetCursorState(true);
        _starterAssetsInputs.SetCursorInputForLook(true);
    }

    public bool IsMenuOpen()
    {
        return _creatureNameUIInstance != null || _breedingUIInstance != null
            || _munchkinpediaUIInstance != null;
    }
}
