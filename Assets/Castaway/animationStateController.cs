using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning; 
    private bool isJumping;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Checks if any of the movement keys are being pressed
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        //Checks if space bar is being pressed
        isJumping = Input.GetKeyDown(KeyCode.Space);

        // Updates the Animator parameters
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);

        // Reset the jump status after a frame to avoid "sticking" in the jumping state
        if (isJumping)
        {
            // After setting the "isJumping" flag to true, reset it in the next frame.
            // This prevents it from staying true after the initial jump input.
            isJumping = false;
        }
    }
}
