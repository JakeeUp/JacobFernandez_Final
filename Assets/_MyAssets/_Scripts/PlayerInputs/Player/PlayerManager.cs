using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    CameraManager cameraManager;
    JumpComponent jump;
    public HitBox hit;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;

    public static PlayerManager instance;

    public GameObject[] playerPieces;

    private void Awake()
    {
        instance = this;
        if (hit != null)
        {
            hit = hit.GetComponent<HitBox>();
        }
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        jump = GetComponent<JumpComponent>();
        
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

      //  isInteracting = animator.GetBool("isInteracting");
        jump.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", jump.isGrounded);
    }

    private void Attack()
    {
        hit.EnableAttack();
    }

    private void AttackDone()
    {
        hit.DisableAttack();
    }
}
