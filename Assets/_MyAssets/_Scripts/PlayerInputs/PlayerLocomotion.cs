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

        if (isJumping || !isGrounded) return;
        moveDirection = cameraObject.forward * inputManager.VerticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.HorizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

       
        moveDirection = moveDirection * runningSpeed;

        if (isGrounded && !isJumping)
        {
            Vector3 movementVelocity = moveDirection;
            rb.velocity = movementVelocity;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            if (inputManager.jump_Input && !isJumping)
            {
                isJumping = true;
                jumpTimeCounter = jumpMaxHoldTime; 
                animatorManager.animator.SetBool("isJumping", true);
                animatorManager.PlayTargetAnimation("standing-jump-start", false);

                float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpingVelocity;
                rb.velocity = playerVelocity;
            }
        }

        if (isJumping && inputManager.jump_Input)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(-2 * gravityIntensity * jumpHeight), rb.velocity.z);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (jumpTimeCounter <= 0 || !inputManager.jump_Input)
        {
            isJumping = false; 
        }
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDir = Vector3.zero;

        targetDir = cameraObject.forward * inputManager.VerticalInput;
        targetDir = targetDir + cameraObject.right * inputManager.HorizontalInput;
        targetDir.Normalize();

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        float yawAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;

        Quaternion yawRotation = Quaternion.Euler(0f, yawAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, yawRotation, rotationSpeed * Time.deltaTime);

        float pitchAngle = Mathf.Atan2(targetDir.y, Mathf.Sqrt(targetDir.x * targetDir.x + targetDir.z * targetDir.z)) * Mathf.Rad2Deg;

        Quaternion pitchRotation = Quaternion.Euler(-pitchAngle, 0f, 0f);
        cam.cameraPivotTransform.localRotation = Quaternion.Slerp(cam.cameraPivotTransform.localRotation, pitchRotation, rotationSpeed * Time.deltaTime);


    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("standing-jump-loop", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer))
        {
            if (!isGrounded && playerManager.isInteracting)
            {
                Debug.Log("Player is now grounded.");

                Debug.Log("Playing standing-jump-end animation.");
                animatorManager.PlayTargetAnimation("standing-jump-end", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
