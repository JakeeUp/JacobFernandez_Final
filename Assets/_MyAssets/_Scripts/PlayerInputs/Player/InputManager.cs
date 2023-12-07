using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputBuffer inputBuffer = new InputBuffer();
    public PlayerControls playerControls;
    public PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;
    JumpComponent jump;
    PlayerStats stats;
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

    [SerializeField] private bool attackInput;
    [SerializeField] private bool previousAttackInput = false;

    [SerializeField] private bool pauseInput;


    private void Awake()
    {

        if (instance == null)
            instance = this;
        playerInput = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        jump = GetComponent<JumpComponent>();
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

            playerControls.PlayerActions.Attack.performed += i => attackInput = true;
            playerControls.PlayerActions.Attack.canceled += i => attackInput = false;

            playerControls.PlayerActions.Pause.performed += i => pauseInput = !pauseInput;
        }

        playerControls.Enable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput(playerLocomotion.isSprinting);
        HandleJumpingInput();
        HandleAttackInput();
        HandlePauseInput();
    }

    private void HandlePauseInput()
    {
        if (pauseInput)
        {

            TogglePause();
            pauseInput = false;
        }
    }

    private void TogglePause()
    {
        GameManager.instance.PauseUnpause();
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump.HandleJumping();
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

    private void HandleSprintingInput(bool trySprint)
    {
        //fix this
        if (b_Input)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }



    }

    public void HandleAttackInput()
    {
        if (attackInput && !previousAttackInput)
        {
            inputBuffer.BufferInput("Attack");
            Debug.Log("trigger");
            
            previousAttackInput = attackInput;
        }

        if (!attackInput && previousAttackInput)
        {
            previousAttackInput = false;
        }
    }
    public bool CheckForBufferedInput(string input)
    {
        if (inputBuffer.HasInput(input))
        {
            inputBuffer.ClearInput();
            return true;
        }
        return false;
    }

    #region encaps
    public float VerticalInput { get { return _verticalInput; } set { _verticalInput = value; } }
    public float HorizontalInput { get { return _horizontalInput; } set { _horizontalInput = value; } }
    public float CameraInputX { get { return _cameraInputX; } set { _cameraInputX = value; } }
    public float CameraInputY { get { return _cameraInputY; } set { _cameraInputY = value; } }
    public float moveAmount { get { return _moveAmount; } set { _moveAmount = value; } }
    #endregion
}