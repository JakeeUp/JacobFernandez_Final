using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static AnimatorManager instance;
    public Animator animator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal = SnapToValue(horizontalMovement);
        float snappedVertical = SnapToValue(verticalMovement);

        if(isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    private float SnapToValue(float value)
    {

        if (value > 0 && value < 0.55f)
        {
            return 0.5f;
        }
        else if (value > 0.55f)
        {
            return 1f;
        }
        else if (value < 0 && value > -0.55f)
        {
            return -0.5f;
        }
        else if (value < -0.55f)
        {
            return -1f;
        }
        return 0f;
    }

}
