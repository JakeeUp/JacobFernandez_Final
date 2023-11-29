using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackComponent : MonoBehaviour
{
    public static KnockBackComponent instance;
    private Rigidbody rb;
    PlayerLocomotion playerLocomotion;

    [SerializeField] public bool isKnocking;
    [SerializeField] public float knockBackLength = 0.5f;
    [SerializeField] public float knockbackCounter;
    [SerializeField] public Vector2 knockbackPower;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void Knockback()
    {
        if (rb != null) 
        {
            isKnocking = true;
            knockbackCounter = knockBackLength;
            Debug.Log("Knocked");

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
                //isKnocking = false;

                playerLocomotion.moveDirection.y = 0; 
            }
        }
    }
}
