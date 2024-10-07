using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private List<GameObject> _interactablesInRange;
    private GameObject _closestInteractable;
    private Base _base;
    private Dictionary<BaitType, int> _baitInventory;
    private List<CreatureData> _tamedCreatureTypes;
    
    public Dictionary<BaitType, int> BaitInventory => _baitInventory;
    public List<CreatureData> TamedCreatures => _tamedCreatureTypes;

    //TODO: change back to 30
    public const float NORMAL_CATCH_RATE = 100;
    public const float TYPE_CATCH_RATE = 70;

    private void Awake()
    {
        _interactablesInRange = new();
        _tamedCreatureTypes = new();

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
        if (_closestInteractable != null)
        {
            _closestInteractable.GetComponent<IInteractable>().Interact();
        }
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
        //Don't add to list if breeding
        if (interactable.TryGetComponent(out Creature creature))
        {
            if (creature.CurrentState == CreatureState.Breeding)
            {
                return;
            }
        }
        
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

    private void TameCreature(CreatureData creature)
    {
        if (!_tamedCreatureTypes.Contains(creature))
        {
            _tamedCreatureTypes.Add(creature);
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
                creature.SetState(CreatureState.FollowPlayer);

                var tamingRewards = creature.CreatureData.TamingReward;
                
                var baitDropIndex = Random.Range(0, tamingRewards.PossibleBaitDrops.Count);
                var randomCount = Random.Range((int)tamingRewards.AmountRange.x, (int)tamingRewards.AmountRange.y + 1);

                _baitInventory[tamingRewards.PossibleBaitDrops[baitDropIndex]] += randomCount;

                TameCreature(creature.CreatureData);
                print($"Gained {randomCount} {tamingRewards.PossibleBaitDrops[baitDropIndex]} Bait!");
            }
            else
            {
                print("Tame Failed");
            }
        }
        else
        {
            if (randomRoll <= TYPE_CATCH_RATE)
            {
                print($"Caught a creature with {baitType} bait");
                creature.SetState(CreatureState.FollowPlayer);
                
                var tamingRewards = creature.CreatureData.TamingReward;
                
                var baitDropIndex = Random.Range(0, tamingRewards.PossibleBaitDrops.Count);
                var randomCount = Random.Range((int)tamingRewards.AmountRange.x, (int)tamingRewards.AmountRange.y + 1);

                _baitInventory[tamingRewards.PossibleBaitDrops[baitDropIndex]] += randomCount;
                
                TameCreature(creature.CreatureData);
                print($"Gained {randomCount} {tamingRewards.PossibleBaitDrops[baitDropIndex]} Bait!");
            }
            else
            {
                print("Tame Failed");
            }
        }
    }
}
