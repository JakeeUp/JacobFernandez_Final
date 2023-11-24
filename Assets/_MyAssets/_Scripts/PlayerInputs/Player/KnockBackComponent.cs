using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackComponent : MonoBehaviour
{
    public static KnockBackComponent instance;

    [SerializeField] bool isKnocking;
    [SerializeField] float knockBackLength = .5f;
    [SerializeField] float knockbackCounter;
    [SerializeField] Vector2 knockbackPower;

    private void Awake()
    {
        instance = this;
    }
    
    private void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockBackLength;
        Debug.Log("Knocked");

    }
}
