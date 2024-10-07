using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour, IInteractable
{
    [SerializeField] private float _visionRange;
    [SerializeField] private Canvas _interactionCanvas;
    [SerializeField] private float _baitSpawnDelay;

    public string CurrentName => _currentName;
    public CreatureState CurrentState => _currentState;
    public CreatureData CreatureData => _creatureData;
    
    private static readonly string AREA_WALKABLE = "Walkable";
    private static readonly string AREA_BASE = "Base";
    
    private NavMeshAgent _navMeshAgent;
    private MeshFilter _mesh;
    private Collider _collider;
    private Player _player;
    private Base _base;

    private CreatureData _creatureData;
    private CreatureState _currentState;
    private Vector3 _basePosition;
    private string _currentName;
    private float _elapsedBaitSpawnTime;
    
    private Dictionary<int, string> _navMeshAreas = new()
    {
        { 0, AREA_WALKABLE },
        { 1, "Not Walkable" },
        { 2, "Jump" },
        { 3, AREA_BASE }
    };

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _mesh = GetComponent<MeshFilter>();
        _player = FindObjectOfType<Player>();
        _base = FindObjectOfType<Base>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _interactionCanvas.gameObject.SetActive(false);
    }
    
    public void Init(CreatureData data)
    {
        _creatureData = data;
        _currentName = data.CreatureName.ToString();

        var creatureModel = Instantiate(data.Model, transform);
    }

    // Update is called once per frame
    void Update()
    {
        _interactionCanvas.transform.forward = _interactionCanvas.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (_currentState == CreatureState.FollowPlayer)
        {
            if (NavMesh.SamplePosition(_navMeshAgent.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                int areaMask = hit.mask;
                string areaName = GetAreaNameFromMask(areaMask);

                if (areaName.Equals(AREA_BASE))
                {
                    SetState(CreatureState.RoamBase);
                    _base.AddCreatureToBase(this);
                }
            }
        }
        
        _updateState();   
    }
    
    string GetAreaNameFromMask(int areaMask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((areaMask & (1 << i)) != 0)
            {
                if (i < _navMeshAreas.Count)
                {
                    return _navMeshAreas[i];
                }
            }
        }
        return "";
    }

    public void SetName(string name)
    {
        _currentName = name;
    }

    private void _updateState()
    {
        switch (_currentState)
        {
            case CreatureState.Untamed:

                if (Vector3.Distance(transform.position, _navMeshAgent.destination) < 0.5f)
                {
                    _navMeshAgent.destination = _pickNewRandomDestination();
                }

                if (_navMeshAgent.velocity.magnitude < 0.01)
                {
                    _navMeshAgent.destination = _pickNewRandomDestination();
                }
                
                break;
            case CreatureState.FollowPlayer:
                
                _navMeshAgent.destination = _player.transform.position;
                
                break;
            case CreatureState.RoamBase:
                
                if (Vector3.Distance(transform.position, _navMeshAgent.destination) < 0.5f)
                {
                    _navMeshAgent.destination = _pickNewRandomDestination();
                }
                
                if (_navMeshAgent.velocity.magnitude < 0.01)
                {
                    _navMeshAgent.destination = _pickNewRandomDestination();
                }

                if (_elapsedBaitSpawnTime >= _baitSpawnDelay)
                {
                    _elapsedBaitSpawnTime = 0;
                    
                    _spawnBait();
                }

                _elapsedBaitSpawnTime += Time.deltaTime;
                
                break;
            case CreatureState.Breeding:
                
                
                
                break;
        }
    }

    public void SetState(CreatureState state)
    {
        _endState();

        _currentState = state;
        
        _startState();
    }

    private void _spawnBait()
    {
        var bait = Instantiate(CreatureData.BaitToSpawn, transform.position,
            CreatureData.BaitToSpawn.transform.rotation);
    }

    private void _endState()
    {
        switch (_currentState)
        {
            case CreatureState.Untamed:
                break;
            case CreatureState.FollowPlayer:
                break;
            case CreatureState.RoamBase:
                break;
            case CreatureState.Breeding:
                break;
        }
    }

    private void _startState()
    {
        switch (_currentState)
        {
            case CreatureState.Untamed:

                string areaToRemove = AREA_BASE;
                
                var areaIndexBase = NavMesh.GetAreaFromName(areaToRemove);

                if (areaIndexBase != -1)
                {
                    int areaMaskToRemove = 1 << areaIndexBase;

                    _navMeshAgent.areaMask &= ~areaMaskToRemove;
                }
                
                _navMeshAgent.speed = 3.5f;
                _navMeshAgent.destination = _pickNewRandomDestination();
                _navMeshAgent.stoppingDistance = 1.5f;
                
                break;
            case CreatureState.FollowPlayer:

                string areaToAdd = AREA_BASE;
                var areaIndexToAdd = NavMesh.GetAreaFromName(areaToAdd);

                _navMeshAgent.areaMask |= (1 << areaIndexToAdd);
                
                _navMeshAgent.speed = 5;
                _navMeshAgent.destination = _player.transform.position;
                _navMeshAgent.stoppingDistance = 4;
                _player.TameCreature(CreatureData);
                
                break;
            case CreatureState.RoamBase:
                
                string areaToRemoveWalk = AREA_WALKABLE;
                
                var areaIndex = NavMesh.GetAreaFromName(areaToRemoveWalk);

                if (areaIndex != -1)
                {
                    int areaMaskToRemove = 1 << areaIndex;

                    _navMeshAgent.areaMask &= ~areaMaskToRemove;
                }
                
                _navMeshAgent.speed = 3.5f;
                _navMeshAgent.stoppingDistance = 1.5f;
                _navMeshAgent.destination = _pickNewRandomDestination();
                
                break;
            case CreatureState.Breeding:

                _navMeshAgent.speed = 0;
                
                break;
        }
    }

    private Vector3 _pickNewRandomDestination()
    {
        var randomAngle = Random.Range(0, 360);
        var randomDistance = Random.Range(2, _visionRange);
        var newDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward;
        var raycastPoint = transform.position + (newDirection.normalized * randomDistance) + (Vector3.up * 10);
        Ray ray = new Ray(raycastPoint, new Vector3(0, -1, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                return hit.point;
            }
        }

        Debug.LogError("No Point Found, Picking new point");
        return _pickNewRandomDestination();
    }

    public void Interact()
    {
        if (_currentState == CreatureState.Untamed)
        {
            if (_player.BaitInventory[_creatureData.BaitNeeded] > 0)
            {
                _player.UseBait(this, _creatureData.BaitNeeded);
            }
            else if (_player.BaitInventory[BaitType.Normal] > 0)
            {
                _player.UseBait(this, BaitType.Normal);
            }
        }
        else if (_currentState == CreatureState.Breeding)
        {
            //Do Nothing
        }
        else
        {
            UIManager.Instance.OpenCreatureNameUI(this);
        }
    }

    public void ShowInteractUI(bool showUI)
    {
        if (_currentState == CreatureState.Breeding)
        {
            showUI = false;
        }
        
        _interactionCanvas.gameObject.SetActive(showUI);

        if (showUI)
        {
            _interactionCanvas.GetComponentInChildren<TMP_Text>().text = $"{_currentName}\n[E] To Interact";
        }
        else if (_currentState != CreatureState.Untamed)
        {
            _interactionCanvas.gameObject.SetActive(true);
            _interactionCanvas.GetComponentInChildren<TMP_Text>().text = _currentName;
        }

        if (_currentState != CreatureState.Untamed)
        {
            _interactionCanvas.GetComponentInChildren<TMP_Text>().color = Color.yellow;
        }
        else
        {
            _interactionCanvas.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddInteractableToList(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.RemoveInteractableFromList(gameObject);
            ShowInteractUI(false);
        }
    }
}

public enum CreatureState
{
    Untamed,
    FollowPlayer,
    RoamBase,
    Breeding
}