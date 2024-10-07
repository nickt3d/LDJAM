using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "CreatureData", menuName = "CreatureData")]
public class CreatureData : ScriptableObject
{ 
    public CreatureName CreatureName;
    public BaseType BaseType;
    public SubType SubType;
    public BaitType BaitNeeded;
    public TamingReward TamingReward;
    public GameObject Model;
    public BaitMonoBehaviour BaitToSpawn;
    public string creatureDescription;
    public Sprite creatureImage;
    public string creatureFavBait;
}

[Serializable]
public class TamingReward
{
    public List<BaitType> PossibleBaitDrops;
    public Vector2 AmountRange;
}

public enum BaseType
{
    Blubber,
    Stoney,
    Trunko
}

public enum SubType
{
    None,
    Flame,
    Slimy,
    Ghostly
}

public enum CreatureName
{
    Bouldy,
    Gremchilla,
    Ghebble,
    Litkit,
    Beer,
    Snozz,
    Grouse,
    Frosgeist,
    Vomoth,
    Flopper,
    Goatus,
    Shoggy
}
