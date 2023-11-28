using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleActions : MonoBehaviour
{
    public PlayerLocomotion playerLocomotion; 
    public Transform leftFootTransform; 
    public Transform rightFootTransform; 
    public LayerMask groundLayer; 
    public LayerMask metalLayer; 
    public LayerMask waterLayer; 
    public float timeSinceLastLeftStep = 0f;
    public float timeSinceLastRightStep = 0f;
    public float leftStepInterval = 0.5f;
    public float rightStepInterval = 0.5f;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Update()
    {
        HandleJumpRumble();

        if (playerLocomotion != null && playerLocomotion.isSprinting)
        {
            HandleLeftFootRumble();
            HandleRightFootRumble();
        }
    }

    private bool isLeftFoot = true;

    private void HandleLeftFootRumble()
    {
        timeSinceLastLeftStep += Time.deltaTime;
        if (timeSinceLastLeftStep >= leftStepInterval)
        {
            timeSinceLastLeftStep = 0f;

            if (Physics.Raycast(leftFootTransform.position, Vector3.down, 0.3f, groundLayer))
            {
                RumbleManager.instance.RumblePulse(0.5f, 0.5f, 0.1f);
            }

            if (Physics.Raycast(leftFootTransform.position, Vector3.down, 0.3f, metalLayer))
            {
                RumbleManager.instance.RumblePulse(0.01f, 0.01f, 0.1f);
            }

            if (Physics.Raycast(leftFootTransform.position, Vector3.down, 0.3f, waterLayer))
            {
                RumbleManager.instance.RumblePulse(1f, 1f, 0.1f);
            }
        }
    }
    private void HandleRightFootRumble()
    {
        timeSinceLastRightStep += Time.deltaTime;
        if (timeSinceLastRightStep >= rightStepInterval)
        {
            timeSinceLastRightStep = 0f;

            if (Physics.Raycast(rightFootTransform.position, Vector3.down, 0.3f, groundLayer))
            {
                RumbleManager.instance.RumblePulse(0.5f, 0.5f, 0.1f);
            }

            if (Physics.Raycast(rightFootTransform.position, Vector3.down, 0.3f, metalLayer))
            {
                RumbleManager.instance.RumblePulse(0.01f, 0.01f, 0.1f);
            }
            if (Physics.Raycast(rightFootTransform.position, Vector3.down, 0.3f, waterLayer))
            {
                RumbleManager.instance.RumblePulse(1f, 1f, 0.1f);
            }
        }
    }

    private static void HandleJumpRumble()
    {
        if (InputManager.instance.playerControls.PlayerActions.Jump.WasPerformedThisFrame())
        {
            RumbleManager.instance.RumblePulse(0.50f, 1f, .1f);
        }
    }

    private void HandleAttackRumble()
    {

    }
}
