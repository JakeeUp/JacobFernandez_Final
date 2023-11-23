using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public CinemachineBrain cineBrain;

    private void Awake()
    {
        instance = this;
        cineBrain = GetComponent<CinemachineBrain>();
    }
}
