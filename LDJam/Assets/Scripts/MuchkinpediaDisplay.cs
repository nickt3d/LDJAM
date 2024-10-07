using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MuchkinpediaDisplay : MonoBehaviour
{
    [SerializeField] public CreatureData munchkin;

    [SerializeField] public TMP_Text nameText;
    [SerializeField] public TMP_Text descriptionText;
    [SerializeField] public Image munchkinImage;
    [SerializeField] public TMP_Text favBait;
    [SerializeField] public TMP_Text baseType;
    [SerializeField] public TMP_Text subType;

    private void Start()
    {
        munchkinImage.gameObject.SetActive(false);
    }

    public void DisplayInformation()
    {
        munchkinImage.gameObject.SetActive(true);

        nameText.text = munchkin.CreatureName.ToString();
        descriptionText.text = munchkin.creatureDescription;
        munchkinImage.sprite = munchkin.creatureImage;
        favBait.text = munchkin.creatureFavBait;
        baseType.text = munchkin.BaseType.ToString();
        subType.text = munchkin.SubType.ToString();     
    }
}
