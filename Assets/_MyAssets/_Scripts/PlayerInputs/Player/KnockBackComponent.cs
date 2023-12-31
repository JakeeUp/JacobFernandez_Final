using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackComponent : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;

    [SerializeField] public bool isKnocking;
    [SerializeField] public float knockBackLength = 0.5f;
    [SerializeField] public float knockbackCounter;
    [SerializeField] public Vector2 knockbackPower;
    [SerializeField] int soundToPlay;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void Knockback()
    {
        if (rb != null) 
        {
            isKnocking = true;
            knockbackCounter = knockBackLength;
            Debug.Log("Knocked");

            AudioManager.instance.PlaySFX(soundToPlay);

            rb.AddForce(new Vector3(0, knockbackPower.y, 0), ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing on the object with KnockBackComponent script.");
        }
    }

    private void Update()
    {
        if (isKnocking)
        {
            knockbackCounter -= Time.deltaTime;
            if (knockbackCounter <= 0)
            {
                isKnocking = false;

                playerLocomotion.moveDirection.y = 0; 
            }
        }
        //if(playerStats.Health <= 0)
        //{
        //    knockbackCounter = 0;
        //}
        
    }
    
}
