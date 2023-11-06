using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
 
    InputManager inputManager;
    CameraManager cam;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

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
    [SerializeField] private float airControlMultiplier = 0.5f; // Adjusts the strength of air control
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
        else // If the player is not grounded, apply air control
        {
            Vector3 airMovement = new Vector3(moveDirection.x * airControlMultiplier, rb.velocity.y, moveDirection.z * airControlMultiplier);
            rb.velocity = Vector3.Lerp(rb.velocity, airMovement, airControl * Time.fixedDeltaTime);
        }
    }

    [SerializeField]private int maxJumpCount = 2; // Set the maximum number of allowed jumps
    [SerializeField]private int currentJumpCount = 0;

  

    public void HandleJumping()
    {
        if ((isGrounded || currentJumpCount < maxJumpCount) && inputManager.jump_Input)
        {
            currentJumpCount++;
            isJumping = true; // Remain true for any jump action.
            jumpTimeCounter = jumpMaxHoldTime; // Reset the timer for holding jump.
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("standing-jump-start", false);

            Jump(); // Perform the jump.

            inputManager.jump_Input = false;
        }

        if (isJumping && inputManager.jump_Input && jumpTimeCounter > 0)
        {
            Jump(); 
            jumpTimeCounter -= Time.deltaTime;
        }

        if (jumpTimeCounter <= 0 || !inputManager.jump_Input)
        {
            // isJumping = false;
        }
    }

    private void Jump()
    {
        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        rb.velocity = new Vector3(rb.velocity.x, jumpingVelocity, rb.velocity.z);
    }

    public void ResetJump()
    {
        currentJumpCount = 0;
        isJumping = false; 
        jumpTimeCounter = 0;
    }
    private void HandleRotation()
    {
        //if (isJumping)
        //    return;

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
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        bool hitGround = Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer);

        if (isGrounded && !hitGround)
        {
            isGrounded = false;
        }

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("standing-jump-loop", true);
            }

            inAirTimer += Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (hitGround && !isGrounded)
        {
            if (hitGround && !isGrounded)
            {
                isGrounded = true;
                isJumping = false; 
                currentJumpCount = 0; 
                animatorManager.PlayTargetAnimation("standing-jump-end", true);
            }
        }
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
