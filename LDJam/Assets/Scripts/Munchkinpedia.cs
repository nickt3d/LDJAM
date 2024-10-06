using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Munchkinpedia : MonoBehaviour
{
    [SerializeField] GameObject munchkinpediaObj;
    bool toggleMunchkinpedia;

    // Start is called before the first frame update
    void Start()
    {
        munchkinpediaObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            toggleMunchkinpedia = !toggleMunchkinpedia;
        }

        if (toggleMunchkinpedia)
        {
            munchkinpediaObj.SetActive(true);
        }
        else if (!toggleMunchkinpedia)
        {
            munchkinpediaObj.SetActive(false);
        }
    }
}
