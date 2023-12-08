using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]Image blackImage;
    [SerializeField] float fadeSpeed = 2f;
    public bool fadeToBlack, fadeFromBlack;

    public TextMeshProUGUI health;
    public Image healthImage;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI specialText;

    public GameObject PauseScreen, OptionsScreen;

    public Slider musicVolSlider, sfxVolSlider;

    public string levelSelect, mainMenu;

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

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    public void OpenOptions()
    {
        OptionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsScreen.SetActive(false);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
    }

    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }
}
