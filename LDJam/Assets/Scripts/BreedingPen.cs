using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingPen : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas _interactionCanvas;

    private Base _base;
    private List<Creature> _creaturesInPen;
    
    // Start is called before the first frame update
    void Start()
    {
        _base = FindObjectOfType<Base>();
        _creaturesInPen = new();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Interact()
    {
        //TODO: Open a menu to pick which creatures are in the pen
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
