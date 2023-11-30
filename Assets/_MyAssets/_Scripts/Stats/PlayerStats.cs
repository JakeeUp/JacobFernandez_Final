using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    private Animator animator;

    RumbleActions rumbleActions;


    [SerializeField]int currentHealth , maxHealth = 5;

     float invincibleLength = 2f;
     float invincCounter;
    public float InvincibleLength { get { return invincibleLength; } set { invincibleLength = value; } }
    public int Health { get { return currentHealth; } set { currentHealth = value; } }
    public float InvincCounter { get { return invincCounter; } set { invincCounter = value; } }
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>(); //get component is not bad if its in awake.
        rumbleActions = GetComponent<RumbleActions>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
    }
    public delegate void HurtPlayer();
    public event HurtPlayer DamagePlayer;

    private void Update()
    {
        PlayerDamageFlashing(); //make this corutine call when taking damage// <<- faster and cleaner 
    }

    private void PlayerDamageFlashing()
    {
        if (invincibleLength > 0)
        {
            invincCounter -= Time.deltaTime;

            for (int i = 0; i < PlayerManager.instance.playerPieces.Length; i++)
            {
                if (Mathf.Floor(invincCounter * 5f) % 2 == 0)
                {
                    PlayerManager.instance.playerPieces[i].SetActive(true);
                }
                else
                {
                    PlayerManager.instance.playerPieces[i].SetActive(false);
                }
                if (invincCounter <= 0)
                {
                    PlayerManager.instance.playerPieces[i].SetActive(true);
                }
            }
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

            KnockBackComponent.instance.Knockback(); //make not a singleton, only 1 exist need for enemies 
            
        }
        
        if(invincCounter < 1f)
        {
            
        }
    }

    public void RespawnResetParams()
    {
        if (currentHealth <= 0)
        {
            KnockBackComponent.instance.knockbackCounter = 0;
            currentHealth = 0;
            GameManager.instance.Respawn();
            RumbleManager.instance.pad.SetMotorSpeeds(0, 0);
            currentHealth = maxHealth;
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }
        else
        {
            invincCounter = invincibleLength;

            
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    //ui health add here//
}
