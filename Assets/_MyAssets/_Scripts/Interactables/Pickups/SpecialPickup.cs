using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPickup : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] GameObject pickupEffect;

    [SerializeField] int soundToPlay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(soundToPlay);
            GameManager.instance.AddSpecial(value);
            HealthUpgrade();
            Destroy(gameObject);
        }
    }

    private void HealthUpgrade()
    {
        if(GameManager.instance.specialCoins >= 3)
        {
            PlayerStats.instance.AddToMaxHealth();
            GameManager.instance.AddSpecial(-3);

        }
    }
}
