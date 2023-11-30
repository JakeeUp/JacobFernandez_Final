using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer
{
    public static InputBuffer instance;
    public float bufferTime = 0.5f;
    private float lastInputTime = float.NegativeInfinity;
    private string lastInput = "";
    private float lastAttackTime;
    public float attackCooldown = 0.5f;

    PlayerLocomotion player;

    public delegate void InputBufferDelegate();
    public InputBufferDelegate playerInputBuffer;

    public delegate void PlayerAttackDelegate();
    public PlayerAttackDelegate playerAttack;



    private void Awake()
    {
        instance = this;
        player = PlayerLocomotion.instance;
        player.Animator = player.GetComponent<Animator>();
        playerInputBuffer += PlayerInputBuffer;
        playerAttack += PlayerAttack;
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
        player.Animator.SetTrigger("Attack");
        AnimatorManager.instance.PlayTargetAnimation("standing-jump-start", true);
        InputManager.instance.inputBuffer.ClearInput();
    }
}
