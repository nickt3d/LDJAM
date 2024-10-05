using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "CreatureData")]
public class CreatureData : ScriptableObject
{
    public string CreatureName;
    public BaseType BaseType;
    public SubType SubType;
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
    Flame,
    Slimy,
    Ghostly
}
