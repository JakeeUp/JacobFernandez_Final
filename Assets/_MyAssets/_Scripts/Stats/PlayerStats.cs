using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    PlayerLocomotion playerMovement;

    private int currentHealth = 100;
    private int maxHealth = 100;

    private float currentMana = 100f;
    private float maximumMana = 100f;

    [SerializeField] private float healthRegenerationRate = 5f;
    [SerializeField] private float manaRegenerationRate = 2f;
    [SerializeField] private float manaDrainRateWhileSprinting = 5f;

    public delegate void HealthChangedEvent(float currentValue);
    public delegate void ManaChangedEvent(float currentValue);

    public event HealthChangedEvent healthChanged;
    public event ManaChangedEvent manaChanged;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerLocomotion>();
        StartCoroutine(RecoverResources());
    }

    private IEnumerator RecoverResources()
    {
        while (true)
        {
            Debug.Log("RecoverResources loop running");
            RegenerateHealth();
            HandleMana();
            yield return null;
        }
    }

    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.FloorToInt(Time.deltaTime * healthRegenerationRate));
            UpdateHealthUI();
        }
    }
    private float manaDrainAccumulator = 0f;
    private void HandleMana()
    {
        if (playerMovement != null && playerMovement.isSprinting)
        {
            ConsumeMana(Time.deltaTime * manaDrainRateWhileSprinting);
        }
        else if (currentMana < maximumMana)
        {
            currentMana = Mathf.Min(maximumMana, currentMana + Time.deltaTime * manaRegenerationRate);
            UpdateManaUI();
        }
    }

    public void AddDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void ConsumeMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0f, maximumMana);
        UpdateManaUI();
    }

    private void UpdateHealthUI()
    {
        float percent = (float)currentHealth / maxHealth;
        healthChanged?.Invoke(percent);
    }

    private void UpdateManaUI()
    {
        float percent = currentMana / maximumMana;
        manaChanged?.Invoke(percent);
    }

    public bool HasEnoughMana(int requiredValue)
    {
        return currentMana >= requiredValue;
    }

    public float CurrentMana
    {
        get { return currentMana; }
    }
}
