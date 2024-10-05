using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CreatureData", menuName = "CreatureData")]
public class CreatureData : ScriptableObject
{
    public string DefaultName;
    public BaseType BaseType;
    public SubType SubType;
    public BaitType BaitNeeded;
    public Mesh Mesh;
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
