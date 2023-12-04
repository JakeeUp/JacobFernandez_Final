using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]Image blackImage;
    [SerializeField] float fadeSpeed = 2f;
    public bool fadeToBlack, fadeFromBlack;

    public TextMeshProUGUI health;
    public Image healthImage;

    public TextMeshProUGUI coinText;

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
        
        if(fadeToBlack)
        {
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, Mathf.MoveTowards(blackImage.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackImage.color.a == 1f)
                fadeToBlack = false;

        }

        if (fadeFromBlack)
        {
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, Mathf.MoveTowards(blackImage.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackImage.color.a == 0f)
                fadeFromBlack = false;

        }
    }
}
