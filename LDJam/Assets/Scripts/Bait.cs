using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BaitMonoBehaviour : MonoBehaviour
{
    [SerializeField] private BaitType _baitType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.BaitInventory[_baitType] += 1;
            Destroy(gameObject);
            
            print($"Picked Up A {_baitType} Bait");
        }
    }
}

public enum BaitType
{
    Normal,
    Blubber,
    Stoney,
    Trunko
}
