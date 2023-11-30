using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterComponent : MonoBehaviour
{
    public delegate void BuoyancyDelegate();
    public BuoyancyDelegate onBuoyancy;

    public delegate void SwimmmingDelegate();
    public SwimmmingDelegate onTransitionToSwimming;

    public delegate void ExitingSwimmingDelegate();
    public ExitingSwimmingDelegate onExitingSwimming;

    PlayerLocomotion player;


    [Header("Swimming Speeds")]
    [SerializeField] float buoyancyForce = 2f;
    [SerializeField] float swimSpeed = 3f;
    [SerializeField] float turnSpeed = 2f;


    private void Awake()
    {
        player = GetComponent<PlayerLocomotion>();
        player.Animator = player.GetComponent<Animator>();
        onBuoyancy += PlayerBuoyancyRate;
        onTransitionToSwimming += PlayerTransitionToSwimming;
        onExitingSwimming += PlayerIsExitingSwimming;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            player.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            player.isInWater = false;
        }
    }

    private void PlayerIsExitingSwimming()
    {
        Debug.Log("Exiting Swimming State");
        player.currentState = PlayerLocomotion.PlayerState.Grounded;

        player.Animator.SetBool("IsSwimming", false);
        JumpComponent.instance.CanPlayerJump();
    }

    private void PlayerTransitionToSwimming()
    {
        player.currentState = PlayerLocomotion.PlayerState.Swimming;
        JumpComponent.instance.isGrounded = false;
        player.Animator.SetBool("IsSwimming", true);
    }

    private void PlayerBuoyancyRate()
    {
        player.rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

        Vector3 swimDirection = transform.forward * InputManager.instance.VerticalInput;
        player.rb.AddForce(swimDirection * swimSpeed, ForceMode.VelocityChange);


        transform.Rotate(Vector3.up, InputManager.instance.HorizontalInput * turnSpeed);
    }

  
}
