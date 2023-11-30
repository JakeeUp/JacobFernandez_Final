using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;


    InputManager input;
    PlayerLocomotion playerLocomotion;

    public Gamepad pad;

    private Coroutine stopRumbleAfterTimeCoroutine;

    [SerializeField]private string currentControlScheme;

    

    private void Awake()
    {
        if (instance == null)
            instance = this;


        input = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Start()
    {
        input.playerInput.onControlsChanged += SwitchControls;
        
    }

   
    
    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        if (currentControlScheme == "Gamepad")
        {
            pad = Gamepad.current;



            if (pad != null)
            {
                pad.SetMotorSpeeds(lowFrequency, highFrequency);
                
                stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumble(duration, pad));
            }
        }
        
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0f, 0f);
    }

    private void SwitchControls(PlayerInput input)
    {
        Debug.Log("device is now: " + input.currentControlScheme);

        currentControlScheme = input.currentControlScheme;
    }

    private void OnDisable()
    {
        input.playerInput.onControlsChanged -= SwitchControls;
    }
}
