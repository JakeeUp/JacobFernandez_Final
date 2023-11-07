using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.inputBuffer.HasInput("Attack"))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
    }
}
