using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private List<GameObject> _interactablesInRange;
    private GameObject _closestInteractable;
    private Base _base;
    private Dictionary<BaitType, int> _baitInventory;

    public Dictionary<BaitType, int> BaitInventory => _baitInventory;

    public const float NORMAL_CATCH_RATE = 30;
    public const float TYPE_CATCH_RATE = 70;

    private void Awake()
    {
        _interactablesInRange = new List<GameObject>();

        _base = FindObjectOfType<Base>();

        _baitInventory = new Dictionary<BaitType, int>()
        {
            { BaitType.Normal, 10 },
            { BaitType.Blubber, 0 },
            { BaitType.Stoney, 0 },
            { BaitType.Trunko, 0 }
        };
    }

    private void Update()
    {
        _updateInteractUI();
        
        if (Input.GetKeyDown(KeyCode.E) && !UIManager.Instance.IsMenuOpen())
        {
            _interact();
        }
    }

    private void _interact()
    {
        _closestInteractable.GetComponent<IInteractable>().Interact();
    }

    private void _updateInteractUI()
    {
        if (_interactablesInRange.Count < 1)
        {
            _closestInteractable = null;
            return;
        }
        
        _closestInteractable = _interactablesInRange[0];
        
        foreach (var interactable in _interactablesInRange)
        {
            interactable.GetComponent<IInteractable>().ShowInteractUI(false);

            if (Vector3.Distance(interactable.transform.position, transform.position)
                < Vector3.Distance(_closestInteractable.transform.position, transform.position))
            {
                _closestInteractable = interactable;
            }
        }
        
        _closestInteractable.GetComponent<IInteractable>().ShowInteractUI(true);
    }

    public void AddInteractableToList(GameObject interactable)
    {
        if (!_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Add(interactable);
        }
    }

    public void RemoveInteractableFromList(GameObject interactable)
    {
        if (_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Remove(interactable);
        }
    }

    public void UseBait(Creature creature, BaitType baitType)
    {
        _baitInventory[baitType] -= 1;
        var randomRoll = Random.Range(1, 101);
        
        if (baitType == BaitType.Normal)
        {
            if (randomRoll <= NORMAL_CATCH_RATE)
            {
                print("Caught a Creature with Normal bait");
                creature.SetState(CreatureState.FollowPlayer);
            }
            else
            {
                print("Catch Failed");
            }
        }
        else
        {
            if (randomRoll <= TYPE_CATCH_RATE)
            {
                print("Caught a creature with special bait");
                creature.SetState(CreatureState.FollowPlayer);
            }
            else
            {
                print("Catch Failed");
            }
        }
    }
}
