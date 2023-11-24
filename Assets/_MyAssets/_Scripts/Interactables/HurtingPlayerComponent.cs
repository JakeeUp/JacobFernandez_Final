using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtingPlayerComponent : MonoBehaviour
{
    PlayerStats stats;
    
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
        if(other.tag == "Player")
        {
            PlayerStats.instance.HurtingPlayer();
        }
    }
}
