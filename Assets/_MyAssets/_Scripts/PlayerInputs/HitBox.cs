using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject hitbox; // Assign your hitbox GameObject in the inspector

    // Call this function to enable the hitbox
    public void EnableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true);
        }
    }

    // Call this function to disable the hitbox
    public void DisableHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthManager>().TakeDamage();
        }
    }
}
