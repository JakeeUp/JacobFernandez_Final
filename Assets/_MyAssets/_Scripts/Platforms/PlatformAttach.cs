using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime;
    private Rigidbody rb;
    private Vector3 currentPos;

    private Rigidbody playerRb;
    private Vector3 lastPlatformPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPlatformPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 previousPos = currentPos;
        currentPos = Vector3.Lerp(startPoint.position, endPoint.position,
            Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -.5f + .5f);
        rb.MovePosition(currentPos);

        // Calculate the difference in platform position
        Vector3 platformDelta = currentPos - previousPos;

        // If the player is on the platform, move them with it
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + platformDelta);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}
