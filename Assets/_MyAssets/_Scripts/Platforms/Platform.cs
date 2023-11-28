using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the platform moves
    public float distance = 5.0f; // Distance the platform moves from the start point

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool movingToEnd = true;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(distance, 0, 0); // Move right by 'distance' units
    }
    private void FixedUpdate()
    {
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
            {
                movingToEnd = false;
            }
        }
        // Move the platform back to the start position
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (transform.position == startPosition)
            {
                movingToEnd = true;
            }
        }
    }

    void Update()
    {
        // Move the platform to the end position
        
    }
}
