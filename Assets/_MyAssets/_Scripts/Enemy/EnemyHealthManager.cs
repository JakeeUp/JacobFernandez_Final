using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth;
    [SerializeField] private int currentHealth;
    public int deathSound;
    public GameObject deathEffect, itemToDrop;
    private Renderer enemyRenderer;
    private Color originalColor;
    public float knockbackForce;
    public float knockbackDuration;
    public float flashDuration = 0.1f;
    [SerializeField] private Vector2 knockbackPower;


    private Rigidbody rb;
    private KnockBackComponent knock;

    private void Awake()
    {
        knock = GetComponent<KnockBackComponent>();
        rb = GetComponent<Rigidbody>();
        enemyRenderer = GetComponentInChildren<Renderer>(); // Assume the enemy has a Renderer component in its children
        originalColor = enemyRenderer.material.color;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {

        currentHealth--;

        // Flash red
        StartCoroutine(FlashRed());

        // Apply knockback
        if (knock != null && rb != null)
        {
            // Example knockback direction calculation: directly backward based on enemy facing
            Vector3 knockbackDirection = -transform.forward;

            // Apply the calculated knockback direction and the force
            rb.AddForce(knockbackDirection.normalized * knockbackPower.x + Vector3.up * knockbackPower.y, ForceMode.Impulse);

            // Perform the knockback
            knock.Knockback();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material.color = originalColor;
    }

    private void Die()
    {
        AudioManager.instance.PlaySFX(deathSound);
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position + new Vector3(0, 1.2f, 0f), transform.rotation);
        Instantiate(itemToDrop, transform.position + new Vector3(0, .5f, 0f), transform.rotation);
    }
}
