using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private float moveAmount;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _horizontalInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMovementInput()
    {
        _verticalInput = movementInput.y;
        _horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }

    #region encaps
    public float VerticalInput { get { return _verticalInput; } set { _verticalInput = value; } }
    public float HorizontalInput { get { return _horizontalInput; } set { _horizontalInput = value; } }
    #endregion
}
