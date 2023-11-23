using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    [Header("Jump Speeds")]

    [SerializeField] private float _jumpHeight = 3;
    [SerializeField] private float _gravityIntensity = -9.8f;

    [Header("Jump Timing")]
    [SerializeField] private float jumpMaxHoldTime = 0.5f;
    private float jumpTimeCounter;

    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int currentJumpCount = 0;

    [SerializeField] private bool jumpButtonReleased = true;

    Rigidbody rb;
    AnimatorManager animatorManager;
    InputManager inputManager;
    PlayerLocomotion player;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isJumping;
    public bool isGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool isJumping { get { return _isJumping; } set { _isJumping = value; } }
    public int MaxJumpCount { get { return maxJumpCount; } }
    public int CurrentJumpCount { get { return currentJumpCount; } set { currentJumpCount = value; } }

    private void Awake()
    {
        player = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    public delegate void JumpingEntryHandler();
    public event JumpingEntryHandler OnJumpingEntry;

    public delegate void ResetJumpHandler();
    public event ResetJumpHandler ResettingJump;

    public void ResetJump()
    {
        ResetJumpHandle();
        ResettingJump?.Invoke();
    }

    public void TriggerJump()
    {
        HandleJumpingEntry();
        OnJumpingEntry?.Invoke();
    }
    private void HandleJumpingEntry()
    {

        Jump();

        jumpTimeCounter = jumpMaxHoldTime;

        animatorManager.animator.SetBool("isJumping", true);
        animatorManager.PlayTargetAnimation("standing-jump-start", true);

    }
    void Update()
    {
        HandleJumpCount();
        HandleJumpInput();
    }

    void FixedUpdate()
    {
        HandleJumping();
    }
    private float jumpCooldown = 0.2f; 
    private float lastJumpTime = -1f;
    void HandleJumpInput()
    {
        bool canJumpAgain = (Time.time - lastJumpTime) >= jumpCooldown;

        if (inputManager.jump_Input && jumpButtonReleased && canJumpAgain)
        {

            if (isGrounded || (player.currentState == PlayerLocomotion.PlayerState.Falling && currentJumpCount < maxJumpCount))
            {
                StartJump();
                lastJumpTime = Time.time; // Reset the last jump time
                jumpButtonReleased = false; // Prevent further jumps until the button is released
            }
        }
        else if (!inputManager.jump_Input)
        {
            jumpButtonReleased = true; // Allow the jump button to be pressed again
        }

        if (!inputManager.jump_Input && player.currentState == PlayerLocomotion.PlayerState.Jumping)
        {
            EndJump();
        }
    }
    public void HandleJumping()
    {
        if (player.currentState == PlayerLocomotion.PlayerState.Jumping)
        {
            if (jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else if (jumpTimeCounter <= 0)
            {
                player.TransitionToState(PlayerLocomotion.PlayerState.Falling);
            }
        }
    }
    private bool jumpInputHandled = false;
    private void HandleJumpCount()
    {
        if (inputManager.jump_Input && !jumpInputHandled)
        {
            currentJumpCount += 1;
            jumpInputHandled = true; 
        }
        else if (!inputManager.jump_Input && jumpInputHandled)
        {
            jumpInputHandled = false;
        }
    }
    void StartJump()
    {
        player.TransitionToState(PlayerLocomotion.PlayerState.Jumping);
        jumpButtonReleased = false;
        
    }

    void EndJump()
    {
        if (jumpTimeCounter <= 0)
        {
            player.TransitionToState(PlayerLocomotion.PlayerState.Falling);
        }
    }

    private void Jump()
    {
        animatorManager.PlayTargetAnimation("standing-jump-start", true);

        float jumpingVelocity = Mathf.Sqrt(-2 * _gravityIntensity * _jumpHeight);
        rb.velocity = new Vector3(rb.velocity.x, jumpingVelocity, rb.velocity.z);
    }
    private void JumpHigher()
    {
        float additionalJumpForce = Mathf.Sqrt(-2 * _gravityIntensity * _jumpHeight) * 0.5f;
        rb.AddForce(Vector3.up * additionalJumpForce, ForceMode.Impulse);
    }

    private void ResetJumpHandle()
    {
        currentJumpCount = 0;
        jumpButtonReleased = true;
        isJumping = false;
        jumpTimeCounter = 0;
        animatorManager.animator.SetBool("isJumping", false);
    }
    public delegate void HandleGroundAfterJump();
    public event HandleGroundAfterJump HandleGrounded;

    public void CanPlayerJump()
    {
        HandleGroundedAfterJump();
        HandleGrounded?.Invoke();
    }

    private void HandleGroundedAfterJump()
    {
        if (inputManager.jump_Input && CurrentJumpCount < MaxJumpCount && isGrounded)
        {
            player.TransitionToState(PlayerLocomotion.PlayerState.Jumping);
        }
    }

}
