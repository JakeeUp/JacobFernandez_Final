using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
   [SerializeField] private GameObject player;
    private Transform referencePoint;

    private void Awake()
    {
        referencePoint = new GameObject("PlatformReferencePoint").transform;
        referencePoint.SetParent(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            player.transform.SetParent(referencePoint);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.transform.SetParent(null);
            player = null;
        }
    }
}
