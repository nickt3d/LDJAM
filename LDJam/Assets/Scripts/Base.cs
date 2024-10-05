using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private List<Creature> _creaturesInBase;

    private void Awake()
    {
        _creaturesInBase = new();
    }
    
    public void AddCreatureToBase(Creature creature)
    {
        _creaturesInBase.Add(creature);
    }
}
