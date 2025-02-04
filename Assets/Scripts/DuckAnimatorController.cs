using System;
using UnityEngine;

public class DuckAnimatorController : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    public float animationOffset = 0.5f;  // Offset in seconds

    private void Start()
    {
        // Set a random animation offset for each duck
        animationOffset = UnityEngine.Random.Range(0f, 1f);  // Adjust the range as needed
        SetAnimationTime();
    }

    private void SetAnimationTime()
    {
        // Ensure the animator is not null
        if (animator != null)
        {
            // Use the Animator's Play method with a normalized time
            animator.Play("updownducklet", -1, animationOffset);
        }
        else
        {
            Console.WriteLine("No animator");
        }
    }
}
