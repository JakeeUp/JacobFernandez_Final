using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    [SerializeField] private float _movementSpeed = 7;
    [SerializeField] private float _rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.hortizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;

        targetDir = cameraObject.forward * inputManager.verticalInput;
        targetDir = targetDir + cameraObject.right * inputManager.hortizontalInput;
        targetDir.Normalize();

        if(targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }

    #region encaps
    public float movementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }
    public float rotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }

    #endregion

}
