using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RumbleActions : MonoBehaviour
{
     PlayerLocomotion playerLocomotion; 
     [SerializeField]Transform leftFootTransform; 
     [SerializeField]Transform rightFootTransform; 
     LayerMask groundLayer; 
     LayerMask metalLayer; 
     LayerMask waterLayer; 
     [SerializeField]float timeSinceLastLeftStep = 0f;
     [SerializeField] float timeSinceLastRightStep = 0f;
     [SerializeField] float leftStepInterval = 0.1f;
     [SerializeField] float rightStepInterval = 0.1f;

    public struct RumbleSettings
    {
        public float lowfrequency;
        public float highfrequency;
        public float duration;
        public RumbleSettings(float lowfrequency, float highfrequency, float duration)
        {
            this.lowfrequency = lowfrequency;
            this.highfrequency = highfrequency;
            this.duration = duration;
        }
    }


    private Dictionary<LayerMask, RumbleSettings> rumbleSettingsDict;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();

        rumbleSettingsDict = new Dictionary<LayerMask, RumbleSettings>
        {
            {LayerMask.NameToLayer("Ground"), new RumbleSettings(0.5f, 0.5f, 0.1f) },
            {LayerMask.NameToLayer("WaterTest"), new RumbleSettings(0.01f, 0.01f, 0.1f) },
            { LayerMask.NameToLayer("Metal"), new RumbleSettings(1f, 1f, 0.1f)}
        };
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

    private void HandleFootRumble(Transform footTransform, ref float timeSinceLastStep, float stepInterval)
    {
        timeSinceLastLeftStep += Time.deltaTime;

        if(timeSinceLastLeftStep >= stepInterval)
        {
            timeSinceLastStep = 0f;

            RaycastHit hit;

            if (Physics.Raycast(footTransform.position,Vector3.down,out hit,0.2f)) 
            {
                int hitLayer = hit.collider.gameObject.layer;
                if(rumbleSettingsDict.ContainsKey(hitLayer))
                {
                    RumbleSettings settings = rumbleSettingsDict[hitLayer];

                    RumbleManager.instance.RumblePulse(settings.lowfrequency,settings.highfrequency, settings.duration);
                }
            }
            
        }
    }
    private void HandleLeftFootRumble()
    {
        HandleFootRumble(leftFootTransform, ref timeSinceLastLeftStep, leftStepInterval);

       
    }
    private void HandleRightFootRumble()
    {

        HandleFootRumble(rightFootTransform, ref timeSinceLastRightStep, rightStepInterval);

    
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
