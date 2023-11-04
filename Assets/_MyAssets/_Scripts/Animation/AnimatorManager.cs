using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        float snappedHorizontal = SnapToValue(horizontalMovement);
        float snappedVertical = SnapToValue(verticalMovement);

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
