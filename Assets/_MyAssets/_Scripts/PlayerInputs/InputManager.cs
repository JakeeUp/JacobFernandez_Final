using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _horizontalInput;


    private void OnEnable()
    {
        if(playerControls == null)
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
    }



    #region encaps
    public float verticalInput { get { return _verticalInput; } set { _verticalInput = value; } }
    public float hortizontalInput { get { return _horizontalInput; } set { _horizontalInput = value; } }

    #endregion
}
