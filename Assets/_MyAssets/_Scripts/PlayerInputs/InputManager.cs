using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 cameraInput;

    [SerializeField] private float _cameraInputX;
    [SerializeField] private float _cameraInputY;
    

    [SerializeField] private float _moveAmount;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _horizontalInput;

    [SerializeField] private bool b_Input;
    [SerializeField] public bool jump_Input;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;
        }

        playerControls.Enable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            playerLocomotion.HandleJumping();
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMovementInput()
    {
        _verticalInput = movementInput.y;
        _horizontalInput = movementInput.x;

        _cameraInputY = cameraInput.y;
        _cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
        
    }
    #region encaps
    public float VerticalInput { get { return _verticalInput; } set { _verticalInput = value; } }
    public float HorizontalInput { get { return _horizontalInput; } set { _horizontalInput = value; } }
    public float CameraInputX { get { return _cameraInputX; } set { _cameraInputX = value; } }
    public float CameraInputY { get { return _cameraInputY; } set { _cameraInputY = value; } }
    public float moveAmount { get { return _moveAmount; } set { _moveAmount = value; } }
    #endregion
}
