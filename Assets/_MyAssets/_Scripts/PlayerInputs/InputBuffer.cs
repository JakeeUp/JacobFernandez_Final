using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer: MonoBehaviour
{
    public static InputBuffer instance;
    public float bufferTime = 0.5f;
    private float lastInputTime = float.NegativeInfinity;
    private string lastInput = "";
    private float lastAttackTime;
    public float attackCooldown = 0.5f;
    private int comboHitCount = 0;
    PlayerLocomotion player;
    Animator animator;
    public delegate void InputBufferDelegate();
    public InputBufferDelegate playerInputBuffer;

    public delegate void PlayerAttackDelegate();
    public PlayerAttackDelegate playerAttack;



    private void Awake()
    {
        instance = this;
        player = PlayerLocomotion.instance;
        animator = GetComponent<Animator>();
        playerInputBuffer += PlayerInputBuffer;
        playerAttack += PlayerAttack;
    }
    public void UpdateBuffer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (InputManager.instance.CheckForBufferedInput("Attack"))
            {
                HandleCombo();
                lastAttackTime = Time.time;
            }
        }

        if (Time.time - lastAttackTime > bufferTime)
        {
            comboHitCount = 0; // Reset combo count
        }
    }

    private void HandleCombo()
    {
        comboHitCount++; 

        comboHitCount = Mathf.Clamp(comboHitCount, 1, 3);

        switch (comboHitCount)
        {
            case 1:
                playerAttack?.Invoke();
                animator.ResetTrigger("Attack2");
                animator.ResetTrigger("Attack3");
                animator.SetTrigger("Attack");
                Debug.Log("attak");
                break;
            case 2:
                playerAttack?.Invoke();
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Attack3");
                animator.SetTrigger("Attack2");
                Debug.Log("attak2");
                break;
            case 3:
                playerAttack?.Invoke();
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("Attack2");
                animator.SetTrigger("Attack3"); 

                Debug.Log("attak3");
                comboHitCount = 0; 
                break;
        }

        lastAttackTime = Time.time; 
        ClearInput(); 
    }

    public void BufferInput(string input)
    {
        lastInput = input;
        lastInputTime = Time.time;
    }

    public bool HasInput(string input)
    {
        if (lastInput == input && (Time.time - lastInputTime) <= bufferTime)
        {
            ClearInput();
            return true;
        }
        return false;
    }


    public void ClearInput()
    {
        lastInput = "";
        lastInputTime = float.NegativeInfinity;
    }

    private void PlayerInputBuffer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (InputManager.instance.CheckForBufferedInput("Attack"))
            {
                player.HandleAttack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void PlayerAttack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("trigger");
        AnimatorManager.instance.PlayTargetAnimation("Attack", true);
        InputManager.instance.inputBuffer.ClearInput();
    }
}
