using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LSUIManager : MonoBehaviour
{
    public static LSUIManager instance;

    public TextMeshProUGUI lNameText;
    public GameObject lNamePanel;

    public TextMeshProUGUI coinsText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
