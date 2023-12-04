using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject deathEffect;
    Vector3 respawnPos;

    public int currentCoins;
    private void Awake()
    {
        instance = this;
       
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        respawnPos = PlayerManager.instance.transform.position;

        AddCoins(0);
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
        KnockBackComponent.instance.knockbackCounter = 0;
        KnockBackComponent.instance.isKnocking = false;

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
}
