using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreedingPen : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas _interactionCanvas;
    [SerializeField] private Creature _creaturePrefab;
    [SerializeField] private float _baseBreedingTime;

    private static readonly string CREATURE_FILE_PATH = "Assets/ScriptableObjects/Creatures";

    public List<Creature> CreaturesInPen => _creaturesInPen;
    
    private Base _base;
    private List<Creature> _creaturesInPen;
    private float _elapsedBreedingTime = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        _base = FindObjectOfType<Base>();
        _creaturesInPen = new();
        
        _interactionCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _interactionCanvas.transform.forward = _interactionCanvas.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (_creaturesInPen.Count < 2)
        {
            _elapsedBreedingTime = 0;
        }
        else
        {
            _elapsedBreedingTime += Time.deltaTime;

            if (_elapsedBreedingTime >= _baseBreedingTime)
            {
                _elapsedBreedingTime = 0;
                
                _breedCreatures();
            }
        }
    }

    private void _breedCreatures()
    {
        List<CreatureData> possibleCreatures = new();
        var guids = AssetDatabase.FindAssets("", new[] { CREATURE_FILE_PATH });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CreatureData asset = AssetDatabase.LoadAssetAtPath<CreatureData>(assetPath);
            
            if (asset != null)
            {
                possibleCreatures.Add(asset);
            }
        }

        int randomCreatureChoice = Random.Range(0, 2);
        int randomTypeChoice = Random.Range(0, 2);

        CreatureName chosenCreatureName = CreaturesInPen[randomCreatureChoice].CreatureData.CreatureName;
        SubType chosenSubType = CreaturesInPen[randomTypeChoice].CreatureData.SubType;

        var selectedCreatureData = possibleCreatures.Find(creature =>
            creature.CreatureName == chosenCreatureName && creature.SubType == chosenSubType);

        if (selectedCreatureData != null)
        {
            var newCreature = Instantiate(_creaturePrefab, transform.position + (transform.forward * 5),
                quaternion.identity);
            
            newCreature.Init(selectedCreatureData);
            newCreature.SetState(CreatureState.RoamBase);
            
            _base.AddCreatureToBase(newCreature);
        }
    }

    public void AddCreatureToPen(Creature creature)
    {
        if (_creaturesInPen.Count < 2)
        {
            _creaturesInPen.Add(creature);
        }
        else
        {
            _base.AddCreatureToBase(_creaturesInPen[0]);
            _creaturesInPen[0] = creature;
        }
    }

    public void TransferCreatureToBase(Creature creature)
    {
        if (_creaturesInPen.Contains(creature))
        {
            _base.AddCreatureToBase(creature);
            _creaturesInPen.Remove(creature);
            creature.SetState(CreatureState.RoamBase);
        }
        else
        {
            Debug.Log("Can't Transfer a Creature that isn't in this pen");
        }
    }

    public void Interact()
    {
        UIManager.Instance.OpenBreedingUI(this);
    }

    public void ShowInteractUI(bool showUI)
    {
        _interactionCanvas.gameObject.SetActive(showUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddInteractableToList(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.RemoveInteractableFromList(gameObject);
            ShowInteractUI(false);
        }
    }
    
    
}
