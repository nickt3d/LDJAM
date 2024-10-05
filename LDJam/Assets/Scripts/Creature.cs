using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour
{
    [SerializeField] private float _visionRange;
    
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
    
    private Dictionary<int, string> _navMeshAreas = new()
    {
        { 0, AREA_WALKABLE },
        { 1, "Not Walkable" },
        { 2, "Jump" },
        { 3, AREA_BASE }
    };
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _mesh = GetComponent<MeshFilter>();
        _player = FindObjectOfType<Player>();
        _base = FindObjectOfType<Base>();
        
        _currentState = CreatureState.Untamed;
    }
    
    public void Init(CreatureData data)
    {
        _creatureData = data;

        if (data.Mesh != null)
        {
            _mesh.mesh = data.Mesh;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentState == CreatureState.Untamed)
            {
                SetState(CreatureState.FollowPlayer);
            }
        }
        
        if (_currentState == CreatureState.FollowPlayer)
        {
            if (NavMesh.SamplePosition(_navMeshAgent.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                // Get the area mask at the sampled position
                int areaMask = hit.mask;

                // Optionally, convert the area mask to a human-readable name
                string areaName = GetAreaNameFromMask(areaMask);

                if (areaName.Equals(AREA_BASE))
                {
                    SetState(CreatureState.RoamBase);
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

    private void _updateState()
    {
        switch (_currentState)
        {
            case CreatureState.Untamed:

                if (Vector3.Distance(transform.position, _navMeshAgent.destination) < 0.5f)
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

                _navMeshAgent.destination = _pickNewRandomDestination();
                _navMeshAgent.stoppingDistance = 0.1f;
                
                break;
            case CreatureState.FollowPlayer:

                _navMeshAgent.destination = _player.transform.position;
                _navMeshAgent.stoppingDistance = 4;
                
                break;
            case CreatureState.RoamBase:

                string areaToRemove = AREA_WALKABLE;

                var areaIndex = NavMesh.GetAreaFromName(areaToRemove);

                if (areaIndex != -1)
                {
                    int areaMaskToRemove = 1 << areaIndex;

                    _navMeshAgent.areaMask &= ~areaMaskToRemove;
                }
                
                _navMeshAgent.stoppingDistance = 0.25f;
                _navMeshAgent.destination = _pickNewRandomDestination();
                
                break;
            case CreatureState.Breeding:
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
}

public enum CreatureState
{
    Untamed,
    FollowPlayer,
    RoamBase,
    Breeding
}