using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait
{
    public BaitType BaitType { get; private set; }

    public Bait(BaitType baitType)
    {
        BaitType = baitType;
    }
}

public enum BaitType
{
    Normal,
    Blubber,
    Stoney,
    Trunko
}
