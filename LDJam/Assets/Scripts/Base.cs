using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private List<Creature> _creaturesInBase;

    public List<Creature> CreaturesInBase => _creaturesInBase;

    private void Awake()
    {
        _creaturesInBase = new();
    }
    
    public void AddCreatureToBase(Creature creature)
    {
        _creaturesInBase.Add(creature);
    }
    
    public void TransferCreatureToBreedingPen(Creature creature, BreedingPen pen)
    {
        if (_creaturesInBase.Contains(creature))
        {
            pen.AddCreatureToPen(creature);
            _creaturesInBase.Remove(creature);
            creature.SetState(CreatureState.Breeding);
        }
        else
        {
            Debug.Log("Can't Transfer a Creature that isn't in your base");
        }
    }
}
