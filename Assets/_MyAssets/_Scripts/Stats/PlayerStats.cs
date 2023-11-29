using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    private Animator animator;

    [SerializeField]int currentHealth , maxHealth = 5;

     float invincibleLength = 2f;
     float invincCounter;
    public float InvincibleLength { get { return invincibleLength; } set { invincibleLength = value; } }
    public float InvincCounter { get { return invincCounter; } set { invincCounter = value; } }
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

    private void Update()
    {
        if(invincibleLength > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }
    public void HurtingPlayer()
    {
        Hurt();
        DamagePlayer?.Invoke();
    }
    private void Hurt()
    {
        if(invincCounter <= 0)
        {
            currentHealth--;
            RespawnResetParams();

            KnockBackComponent.instance.Knockback();
        }
        
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
        else
        {
            invincCounter = invincibleLength;
        }
    }


}
