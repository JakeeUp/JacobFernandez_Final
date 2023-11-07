using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    public enum PlayerState
    {
        Grounded,
        Jumping,
        Falling
    }

    public PlayerState currentState;

    PlayerManager playerManager;
    AnimatorManager animatorManager;
    Animator animator;
 
    InputManager inputManager;
    CameraManager cam;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;
    private float lastAttackTime;
    public float attackCooldown = 0.5f;
    [Header("Falliong")]
    [SerializeField] private float _inAirTimer;
    [SerializeField] private float _leapingVelocity;
    [SerializeField] private float _fallingVelocity;
    [SerializeField] private float rayCastHeightOffset = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    public float maxDistance = 1;

    [Header("Movement Flags")]
    [SerializeField] private bool _isSprinting; 
    [SerializeField] private bool _isGrounded; 
    [SerializeField] private bool _isJumping; 


    [Header("Movement Speeds")]
    [SerializeField] private float _runningSpeed = 7;
    [SerializeField] private float _rotationSpeed = 15;
    [SerializeField] private float _walkingSpeed = 1.5f;
    [SerializeField] private float _sprintingSpeed = 7.5f;

    [Header("Jump Speeds")]

    [SerializeField] private float _jumpHeight = 3;
    [SerializeField] private float _gravityIntensity = -9.8f;

    [Header("Air Control")]
    [SerializeField] private float airControlMultiplier = 0.5f; 
    [SerializeField] private float airControl = 2f;
    [SerializeField] private float airRotationSpeed = 5f;

    [Header("Jump Timing")]
    [SerializeField] private float jumpMaxHoldTime = 0.5f;
    private float jumpTimeCounter;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        cam = FindObjectOfType<CameraManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Grounded:
                HandleGrounded();
                break;
            case PlayerState.Jumping:
                HandleJumping();
                break;
            case PlayerState.Falling:
                HandleFalling();
                break;
        }

        HandleFallingAndLanding();
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (inputManager.CheckForBufferedInput("Attack"))
            {
                HandleAttack();
                lastAttackTime = Time.time; 
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Falling || currentState == PlayerState.Jumping)
        {
            ApplyAirControl();
        }
    }

    private void HandleFalling()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            TransitionToState(PlayerState.Grounded);
        }
    }
    
    private void TransitionToState(PlayerState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case PlayerState.Grounded:
                HandleGroundedEntry();
                break;
            case PlayerState.Jumping:
                HandleJumpingEntry();
                break;
            case PlayerState.Falling:
                HandleFallingEntry();
                break;
        }
    }

    public void HandleAttack()
    {
        animator.SetTrigger("Attack");
        animatorManager.PlayTargetAnimation("standing-jump-start", true);
        inputManager.inputBuffer.ClearInput();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if(playerManager.isInteracting)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }
    private void HandleGroundedEntry()
    {
        ResetJump();
        animatorManager.animator.SetBool("isJumping", false);
        animatorManager.PlayTargetAnimation("standing-jump-end", true);
    }

   
    private void HandleGrounded()
    {
        if (inputManager.jump_Input && currentJumpCount < maxJumpCount && isGrounded)
        {
            TransitionToState(PlayerState.Jumping);
        }
    }
    private void HandleFallingEntry()
    {
        animatorManager.animator.SetBool("isJumping", false);
        animatorManager.PlayTargetAnimation("standing-jump-loop", true);
    }

    private void ApplyAirControl()
    {
        Vector3 airMovement = cameraObject.TransformDirection(new Vector3(inputManager.HorizontalInput, 0, inputManager.VerticalInput));

        float currentAirControlMultiplier = airControlMultiplier;
        if (!inputManager.jump_Input)
        {
            // Option 1: Reduced air control
            //currentAirControlMultiplier *= 0.5f; // Reduce the air control by half, for example

            // Option 2: Increased gravity (if your character's Rigidbody uses gravity)
           rb.AddForce(Physics.gravity * Time.fixedDeltaTime);

            // Option 3: Apply drag (reducing horizontal velocity over time)
            //rb.velocity = new Vector3(rb.velocity.x * 0.95f, rb.velocity.y, rb.velocity.z * 0.95f); // Apply drag
        }

        airMovement *= currentAirControlMultiplier;

        rb.velocity = new Vector3(
            Mathf.Lerp(rb.velocity.x, airMovement.x * airControl, Time.fixedDeltaTime),
            rb.velocity.y, 
            Mathf.Lerp(rb.velocity.z, airMovement.z * airControl, Time.fixedDeltaTime)
        );
    }
    private void HandleMovement()
    {

        moveDirection = cameraObject.forward * inputManager.VerticalInput;
        moveDirection += cameraObject.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float targetSpeed = isSprinting ? sprintingSpeed : inputManager.moveAmount >= 0.5f ? runningSpeed : walkingSpeed;
        moveDirection *= targetSpeed;

        if (isGrounded)
        {
            Vector3 movementVelocity = moveDirection;
            rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
        }
        else 
        {
            Vector3 airMovement = new Vector3(moveDirection.x * airControlMultiplier, rb.velocity.y, moveDirection.z * airControlMultiplier);
            rb.velocity = Vector3.Lerp(rb.velocity, airMovement, airControl * Time.fixedDeltaTime);
        }
    }

    [SerializeField]private int maxJumpCount = 2; 
    [SerializeField]private int currentJumpCount = 0;


    [SerializeField]private bool jumpButtonReleased = true;

    #region Jump Functions
    private void HandleJumpingEntry()
    {
        if (!isGrounded)
        {
            currentJumpCount++;
        }

        Jump();

        jumpTimeCounter = jumpMaxHoldTime;

        animatorManager.animator.SetBool("isJumping", true);
        animatorManager.PlayTargetAnimation("standing-jump-start", true);

    }
    public void HandleJumping()
    {
        if (inputManager.jump_Input && jumpButtonReleased)
        {
            if (isGrounded)
            {
                TransitionToState(PlayerState.Jumping);
                jumpButtonReleased = false; // Player must release the button before it registers another jump
            }
            else if (currentState == PlayerState.Falling && currentJumpCount < maxJumpCount)
            {
                TransitionToState(PlayerState.Jumping);
                jumpButtonReleased = false; // Similar as above
            }
        }

        if (inputManager.jump_Input && jumpTimeCounter > 0)
        {
            jumpTimeCounter -= Time.deltaTime;
        }
        else if (jumpTimeCounter <= 0 && currentState != PlayerState.Falling)
        {
            TransitionToState(PlayerState.Falling);
        }

        if (!inputManager.jump_Input)
        {
            jumpButtonReleased = true;
        }
    }

    private void Jump()
    {
        animatorManager.PlayTargetAnimation("standing-jump-start", true);

        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        rb.velocity = new Vector3(rb.velocity.x, jumpingVelocity, rb.velocity.z);
    }
    private void JumpHigher()
    {
        float additionalJumpForce = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight) * 0.5f; 
        rb.AddForce(Vector3.up * additionalJumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        currentJumpCount = 0;
        jumpButtonReleased = true;
        isJumping = false;
        jumpTimeCounter = 0;
        animatorManager.animator.SetBool("isJumping", false);
    }

    #endregion
    private void HandleRotation()
    {
        Vector3 targetDir = cameraObject.forward * inputManager.VerticalInput;
        targetDir += cameraObject.right * inputManager.HorizontalInput;
        targetDir.Normalize();

        if (targetDir.sqrMagnitude > 0.1f)
        {
            targetDir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);

            float usedRotationSpeed = isGrounded ? rotationSpeed : airRotationSpeed;

            Debug.Log($"Rotating with speed: {usedRotationSpeed} - IsGrounded: {isGrounded}");

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, usedRotationSpeed * Time.deltaTime);
        }

    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + Vector3.up * rayCastHeightOffset;
        isGrounded = Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, maxDistance, groundLayer);

        if (isGrounded && currentState != PlayerState.Grounded)
        {
            TransitionToState(PlayerState.Grounded);
        }
        else if (!isGrounded && currentState != PlayerState.Falling && !isJumping)
        {
            TransitionToState(PlayerState.Falling);
        }
    }
    private IEnumerator SetGroundedWithDelay()
    {
        yield return new WaitForSeconds(0.1f); 
        isGrounded = true;
        if (currentState != PlayerState.Grounded)
        {
            TransitionToState(PlayerState.Grounded);
        }
    }

    public bool IsCharacterMoving()
    {
        return (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f);
    }

    #region encaps

    //floats 
    public float runningSpeed { get { return _runningSpeed; } set { _runningSpeed = value; } }
    public float rotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }
    public float walkingSpeed { get { return _walkingSpeed; } set { _walkingSpeed = value; } }
    public float sprintingSpeed { get { return _sprintingSpeed; } set { _sprintingSpeed = value; } }
    public float inAirTimer { get { return _inAirTimer; } set { _inAirTimer = value; } }
    public float leapingVelocity { get { return _leapingVelocity; } set { _leapingVelocity = value; } }
    public float fallingVelocity { get { return _fallingVelocity; } set { _fallingVelocity = value; } }
    public float jumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float gravityIntensity { get { return _gravityIntensity; } set { _gravityIntensity = value; } }

    //bools
    public bool isSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
    public bool isGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
    public bool isJumping { get { return _isJumping; } set { _isJumping = value; } }

    #endregion

}
