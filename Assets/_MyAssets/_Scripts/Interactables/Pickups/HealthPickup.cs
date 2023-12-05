using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]int healAmount;
    [SerializeField]bool isFullHeal;
    [SerializeField] int soundToPlay;
    public GameObject healthPickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject effect = Instantiate(healthPickupEffect, PlayerManager.instance.transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity);

            AudioManager.instance.PlaySFX(soundToPlay);
            
            effect.transform.SetParent(PlayerManager.instance.transform, true);

            Destroy(gameObject);

            if (isFullHeal)
            {
                PlayerStats.instance.ResetHealth();
            }
            else
            {
                PlayerStats.instance.AddHealth(healAmount);
            }

        }
    }



}
