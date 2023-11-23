using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatsMenu : MonoBehaviour
{
    public UIStatusBar healthBar;
    public UIStatusBar manaBar;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.healthChanged += healthBar.ChangeValue;
        playerStats.manaChanged += manaBar.ChangeValue;
    }
}
