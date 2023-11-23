using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    Vector3 respawnPos;
    private void Awake()
    {
        instance = this;
       
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        respawnPos = PlayerManager.instance.transform.position;
    }
    public void Respawn()
    {
        StartCoroutine(RespawnWaitTime());
    }

    IEnumerator RespawnWaitTime()
    {
        PlayerManager.instance.gameObject.SetActive(false);
        CameraController.instance.cineBrain.enabled = false;

        UIManager.instance.fadeToBlack = true;
        yield return new WaitForSeconds(2f);
        UIManager.instance.fadeFromBlack = true; 
        PlayerManager.instance.transform.position = respawnPos;
        CameraController.instance.cineBrain.enabled = true;
        PlayerManager.instance.gameObject.SetActive(true);
    }
}
