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
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private GameObject munchkinpediaIcon;
    
    public static UIManager Instance { get; private set; }

    private StarterAssetsInputs _starterAssetsInputs;
    private CreatureNameUI _creatureNameUIInstance;
    private BreedingUI _breedingUIInstance;
    private Munchkinpedia _munchkinpediaUIInstance;
    private PauseUI _pauseUIInstance;
    
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
        
        _onMenuClose();
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

            if (_munchkinpediaUIInstance != null)
            {
                Destroy(_munchkinpediaUIInstance.gameObject);    
                _onMunchkinpediaUIClose();
            }

            if (_pauseUIInstance != null)
            {
                Destroy(_pauseUIInstance.gameObject);
                _onPauseUIClose();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenMunchkinpediaUI();
            munchkinpediaIcon.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenPauseUI();
        }
    }

    public void OpenCreatureNameUI(Creature creature)
    {
        if (!IsMenuOpen())
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
        if (!IsMenuOpen())
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
        if (!IsMenuOpen())
        {
            _starterAssetsInputs.SetCursorState(false);
            _starterAssetsInputs.SetCursorInputForLook(false);

            _munchkinpediaUIInstance = Instantiate(_munchkinpediaUI, transform);

            _munchkinpediaUIInstance.OnMenuClose += _onMunchkinpediaUIClose;
        }
    }

    private void OpenPauseUI()
    {
        if (!IsMenuOpen())
        {
            _starterAssetsInputs.SetCursorState(false);
            _starterAssetsInputs.SetCursorInputForLook(false);

            _pauseUIInstance = Instantiate(_pauseUI, transform);

            _pauseUIInstance.OnMenuClose += _onPauseUIClose;
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

        munchkinpediaIcon.SetActive(true);

        _onMenuClose();
    }

    private void _onPauseUIClose()
    {
        _pauseUIInstance.OnMenuClose -= _onPauseUIClose;
        _pauseUIInstance = null;

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
            || _munchkinpediaUIInstance != null || _pauseUIInstance != null;
    }
}
