using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleActions : MonoBehaviour
{
    public PlayerLocomotion playerLocomotion; // Reference to the locomotion script
    public Transform leftFootTransform; // Assign this to the player's feet position in the inspector
    public Transform rightFootTransform; // Assign this to the player's feet position in the inspector
    public LayerMask groundLayer; // Set this in the inspector to the layer that the ground is on
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
            Debug.Log("run rumble called");
        }
        Debug.Log("run rumble called");
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
                Debug.Log("left foot run rumble");
                RumbleManager.instance.RumblePulse(0.35f, 0.35f, 0.1f);
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
                Debug.Log("right foot run rumble");
                RumbleManager.instance.RumblePulse(0.35f, 0.35f, 0.1f);
            }
        }
    }

    private static void HandleJumpRumble()
    {
        if (InputManager.instance.playerControls.PlayerActions.Jump.WasPerformedThisFrame())
        {
            Debug.Log("jump rumble");
            RumbleManager.instance.RumblePulse(0.50f, 1f, .1f);
        }
    }
}
