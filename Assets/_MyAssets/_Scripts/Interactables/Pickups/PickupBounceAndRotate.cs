using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBounceAndRotate : MonoBehaviour
{
    [SerializeField]float rotationSpeed = 10f;
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float moveAmount = 1f;

    float originalY;
    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        float newY = originalY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;

        transform.position = new Vector3(transform.position.x,newY,transform.position.z);
    }
}
