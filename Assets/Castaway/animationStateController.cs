using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    private bool isJumping;
    public bool isThrowingRod; // holding left click
    private bool isIdle;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for movement (running)
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        // Check if left mouse is held (for throwing rod)
        isThrowingRod = Input.GetMouseButton(0);
        animator.SetBool("isThrowingRod", isThrowingRod);

        // Compute idle status
        isIdle = !isRunning && !isJumping;

        // If the player is throwing the rod, don't set the regular movement animations
        if (isThrowingRod)
        {
            animator.SetBool("isThrowingRunning", isRunning);
            animator.SetBool("isThrowingJumping", isJumping);
        }
        else
        {
            // Only set running/jumping/idle if NOT throwing rod
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isIdle", isIdle);
        }

        // Reset jump after one frame to prevent staying in jump state
        if (isJumping)
        {
            isJumping = false;
        }
    }
}
