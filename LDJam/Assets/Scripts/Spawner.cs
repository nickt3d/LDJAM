using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Creature _creaturePrefab;
    [SerializeField] private List<CreatureData> _possibleSpawns;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnRadius;

    private float _elapsedSpawnDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        _elapsedSpawnDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedSpawnDelay += Time.deltaTime;

        if (_elapsedSpawnDelay >= _spawnDelay)
        {
            _elapsedSpawnDelay = 0;

            _spawnCreature();
        }
    }
    
    private void _spawnCreature()
    {
        var randomCreatureSelection = Random.Range(0, _possibleSpawns.Count);

        var newCreature = Instantiate(_creaturePrefab,
            transform.position + new Vector3(Random.Range(0, _spawnRadius), 5, Random.Range(0, _spawnRadius)),
            Quaternion.identity);
        
        newCreature.Init(_possibleSpawns[randomCreatureSelection]);
    }
}
