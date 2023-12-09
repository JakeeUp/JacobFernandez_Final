using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamagePoint : MonoBehaviour
{
    public int clank;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BossController.instance.DamageBoss();
        }
        if(other.tag == "Hitbox")
        {
            AudioManager.instance.PlaySFX(clank);

        }
    }
}
