using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointComponent : MonoBehaviour
{
    [SerializeField] GameObject cpOn, cpOff;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.SetSpawnPoint(transform.position);

            cpOff.SetActive(false);
            cpOn.SetActive(true);
        }
    }
}
