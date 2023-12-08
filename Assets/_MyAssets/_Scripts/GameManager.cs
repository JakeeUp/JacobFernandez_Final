using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    KnockBackComponent knock;
    public GameObject deathEffect;
    Vector3 respawnPos;

    [SerializeField] int levelEndMusic = 2;
    [SerializeField] string levelToLoad;

    public int currentCoins;
    public int specialCoins;
    private void Awake()
    {
        instance = this;
        knock = FindObjectOfType<KnockBackComponent>();
    }
    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
        respawnPos = PlayerManager.instance.transform.position;

        AddCoins(0);
        AddSpecial(0);
    }
    public void Respawn()
    {
        StartCoroutine(RespawnWaitTime());
        PlayerStats.instance.PlayerKilled();

    }

    IEnumerator RespawnWaitTime()
    {
        PlayerManager.instance.gameObject.SetActive(false);
        CameraController.instance.cineBrain.enabled = false;

        UIManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerManager.instance.transform.position + new Vector3 (0f,2f,0f), PlayerManager.instance.transform.rotation);


        yield return new WaitForSeconds(2f);

        PlayerStats.instance.ResetHealth();
        knock.knockbackCounter = 0;
        knock.isKnocking = false;

        UIManager.instance.fadeFromBlack = true; 
        PlayerManager.instance.transform.position = respawnPos;
        CameraController.instance.cineBrain.enabled = true;
        PlayerManager.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPos = newSpawnPoint;
        Debug.Log("spawn point set");
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        UIManager.instance.coinText.text = ":" + currentCoins;
    }
    public void AddSpecial(int specialToAdd)
    {
        specialCoins += specialToAdd;
        UIManager.instance.specialText.text = ":" + specialCoins;
    }
    public void PauseUnpause()
    {
        if(UIManager.instance.PauseScreen.activeInHierarchy)
        {
            UIManager.instance.PauseScreen.SetActive(false);
            Time.timeScale = 1;
            UpdateCursorState(false);
        }
        else
        {
            UIManager.instance.PauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();
            Time.timeScale = 0;
            UpdateCursorState(true);
        }
    }

    public void UpdateCursorState(bool isGamePaused)
    {
        if (isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public IEnumerator LevelEndCo()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerLocomotion.instance.DisableMovement();
        PlayerLocomotion.instance.Animator.SetBool("isLevelEnd", true);
        yield return new WaitForSeconds(4.5f);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        Debug.Log("level ended");


        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_coins"))
        {
            if (currentCoins > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_coins"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
        }
        SceneManager.LoadScene(levelToLoad);



    }

   

}
