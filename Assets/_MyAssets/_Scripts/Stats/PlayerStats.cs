using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    KnockBackComponent knock;
    [SerializeField] int soundToPlay;

    public static PlayerStats instance;

    private Animator animator;
    UIManager uIManager;

    RumbleActions rumbleActions;


    [SerializeField]int currentHealth , maxHealth = 5;

     float invincibleLength = 2f;
     float invincCounter;

    public Sprite[] healthImages;
    public float InvincibleLength { get { return invincibleLength; } set { invincibleLength = value; } }
    public int Health { get { return currentHealth; } set { currentHealth = value; } }
    public float InvincCounter { get { return invincCounter; } set { invincCounter = value; } }
    private void Awake()
    {
        instance = this;
        uIManager = GetComponent<UIManager>();
        animator = GetComponent<Animator>(); //get component is not bad if its in awake.
        knock = GetComponent<KnockBackComponent>();
        rumbleActions = GetComponent<RumbleActions>();
    }

    private void Start()
    {
        ResetHealth();


    }
    public delegate void HurtPlayer();
    public event HurtPlayer DamagePlayer;

    private void Update()
    {
        PlayerDamageFlashing(); //make this corutine call when taking damage// <<- faster and cleaner 
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIManager.instance.healthImage.enabled = true;

        UpdateUI();
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

            knock.Knockback(); //make not a singleton, only 1 exist need for enemies 
            
        }
        
        if(invincCounter < 1f)
        {
            
        }
    }

    public void RespawnResetParams()
    {
        if (currentHealth <= 0)
        {
            knock.knockbackCounter = 0;
            currentHealth = 0;
            GameManager.instance.Respawn();
            //RumbleManager.instance.pad.SetMotorSpeeds(0, 0);
            currentHealth = maxHealth;
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }
        else
        {
            invincCounter = invincibleLength;

            
        }
        UpdateUI();
    }
    
    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        UIManager.instance.health.text = currentHealth.ToString();

        switch (currentHealth)
        {
            case 5:
                UIManager.instance.healthImage.sprite = healthImages[4];
                break;
            case 4:
                UIManager.instance.healthImage.sprite = healthImages[3];
                break;
            case 3:
                UIManager.instance.healthImage.sprite = healthImages[2];
                break;
            case 2:
                UIManager.instance.healthImage.sprite = healthImages[1];
                break;
            case 1:
                UIManager.instance.healthImage.sprite = healthImages[0];
                break;

            case 0:
                UIManager.instance.healthImage.enabled = false;
                break;
        }
    }


    public void PlayerKilled()
    {
        currentHealth = 0;
        AudioManager.instance.PlaySFX(soundToPlay);
        UpdateUI();
    }

    public void AddToMaxHealth()
    {
        maxHealth++;
        currentHealth = maxHealth;
        Debug.Log("Health Upgrade");
        UpdateUI();
    }
        //ui health add here//
}
