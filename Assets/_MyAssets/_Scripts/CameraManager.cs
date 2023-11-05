using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] private Transform targetTransform;
    [SerializeField] public Transform cameraPivotTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask collisionLayers;

    [SerializeField] private float defaultPosition;
    [SerializeField] private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] private Vector3 cameraVectorPosition;

    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float minCollisionOffeset = 0.2f;
    [SerializeField] private float cameraCollisionOffset = 0.2f;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private float cameraLookSpeed = 2;
    [SerializeField] private float cameraPivotSpeed = 2;

    [SerializeField] private float lookAngle;
    [SerializeField] private float pivotAngle;
    [SerializeField] private float minPivotAngle = -35;
    [SerializeField] private float maxPivotAngle = 35;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;

        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;

        lookAngle = lookAngle + (inputManager.CameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.CameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;

    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivotTransform.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition)< minCollisionOffeset)
        {
            targetPosition = targetPosition - minCollisionOffeset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
