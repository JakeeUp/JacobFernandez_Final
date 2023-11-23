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
    JumpComponent playerJump;
    PlayerStats stats;

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


    [Header("Movement Speeds")]
    [SerializeField] private float _runningSpeed = 7;
    [SerializeField] private float _rotationSpeed = 15;
    [SerializeField] private float _walkingSpeed = 1.5f;
    [SerializeField] private float _sprintingSpeed = 7.5f;

 

    [Header("Air Control")]
    [SerializeField] private float airControlMultiplier = 0.5f; 
    [SerializeField] private float airControl = 2f;
    [SerializeField] private float airRotationSpeed = 5f;

    

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        cam = FindObjectOfType<CameraManager>();
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<JumpComponent>();
        cameraObject = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Grounded:
                playerJump.CanPlayerJump();
                break;
            case PlayerState.Jumping:
                playerJump.HandleJumping();
                break;
            case PlayerState.Falling:
                HandleFalling();
                break;
        }

        HandleFallingAndLanding();
        HandleGrounded(playerJump);
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (inputManager.CheckForBufferedInput("Attack"))
            {
                HandleAttack();
                lastAttackTime = Time.time; 
            }
        }
    }
    public void UpdateSprinting(bool trySprint)
    {
        if (trySprint && stats.CurrentMana > 20)
        {
            // Sprinting logic here
            isSprinting = true;
        }
        else
        {
            // Stop sprinting
            isSprinting = false;
        }
    }
    private void FixedUpdate()
    {
        HandleFalling();

        if (currentState == PlayerState.Falling || currentState == PlayerState.Jumping)
        {
            ApplyAirControl();
        }
    }

    private void HandleFalling()
    {
        if (!playerJump.isGrounded)
        {
            // If the player is falling and not yet grounded, increase the falling velocity
            _fallingVelocity += Physics.gravity.y * fallingVelocity * Time.deltaTime;
        }
        else
        {
            // If the player is grounded, reset the falling velocity
            _fallingVelocity = 0;
        }

        // Apply the falling velocity to the player's Rigidbody
        if (currentState == PlayerState.Falling)
        {
            Vector3 downwardVelocity = new Vector3(0, _fallingVelocity, 0);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + downwardVelocity.y, rb.velocity.z);
        }
        //if (playerJump.isGrounded && rb.velocity.y <= 0)
        //{
        //    TransitionToState(PlayerState.Grounded);
        //}
    }
    
    public void TransitionToState(PlayerState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case PlayerState.Grounded:
                HandleGroundedEntry();
                break;
            case PlayerState.Jumping:
                if (playerJump != null)
                    playerJump.TriggerJump();
                else
                    Debug.Log("jump not ref");
                break;
            case PlayerState.Falling:
                _fallingVelocity = 0;
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
        playerJump.ResetJump();
        playerJump.CurrentJumpCount = 0; // Reset the jump count when grounded
        _fallingVelocity = 0; // Reset falling velocity when grounded

        animatorManager.animator.SetBool("isJumping", false);
        animatorManager.PlayTargetAnimation("standing-jump-end", true);
    }

   
    private void HandleGrounded(JumpComponent jump)
    {
        jump.CanPlayerJump();
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

        if (playerJump.isGrounded)
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

   

    #region Jump Functions
   
   
   
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

            float usedRotationSpeed = playerJump.isGrounded ? rotationSpeed : airRotationSpeed;

            Debug.Log($"Rotating with speed: {usedRotationSpeed} - IsGrounded: {playerJump.isGrounded}");

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, usedRotationSpeed * Time.deltaTime);
        }

    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position + Vector3.up * rayCastHeightOffset;
        playerJump.isGrounded = Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, maxDistance, groundLayer);

        if (playerJump.isGrounded && currentState != PlayerState.Grounded)
        {
            TransitionToState(PlayerState.Grounded);
        }
        else if (!playerJump.isGrounded && currentState != PlayerState.Falling && !playerJump.isJumping)
        {
            TransitionToState(PlayerState.Falling);
        }
    }
    void OnDrawGizmos()
    {
        if (!UnityEditor.Selection.Contains(gameObject)) return;

        Vector3 rayCastOrigin = transform.position + Vector3.up * rayCastHeightOffset;
        Vector3 rayCastDirection = Vector3.down * maxDistance;

        Gizmos.color = playerJump.isGrounded ? Color.green : Color.red;

        Gizmos.DrawRay(rayCastOrigin, rayCastDirection);

        Gizmos.DrawWireSphere(rayCastOrigin, 0.2f);

        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out RaycastHit hit, maxDistance, groundLayer))
        {
            Gizmos.DrawWireSphere(rayCastOrigin + Vector3.down * hit.distance, 0.2f);
        }
    }

    private IEnumerator SetGroundedWithDelay()
    {
        yield return new WaitForSeconds(0.1f); 
        playerJump.isGrounded = true;
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
  

    //bools
    public bool isSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
  

    #endregion

}
