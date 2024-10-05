using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingPen : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas _interactionCanvas;

    public List<Creature> CreaturesInPen => _creaturesInPen;
    
    private Base _base;
    private List<Creature> _creaturesInPen;
    
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
