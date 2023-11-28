using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterComponent : MonoBehaviour
{
    PlayerLocomotion player;

    private void Awake()
    {
        player = GetComponent<PlayerLocomotion>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            player.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            player.isInWater = false;
        }
    }
}
