using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    private Animator animator;

    [SerializeField]int currentHealth , maxHealth = 5;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
    }
    public delegate void HurtPlayer();
    public event HurtPlayer DamagePlayer;

    public void HurtingPlayer()
    {
        Hurt();
        DamagePlayer?.Invoke();
    }
    private void Hurt()
    {
        currentHealth--;
        RespawnResetParams();
        

    }

    private void RespawnResetParams()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.instance.Respawn();
            currentHealth = maxHealth;
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }
    }


}
