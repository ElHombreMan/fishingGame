using System.Collections;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    private bool isJumping;
    public bool isThrowingRod;
    private bool isIdle;

    private bool hasCastRod = false;
    private bool isResettingFromCast = false;
    private bool canCast = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movement input
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        // If we just finished casting, press again to reset
        if (hasCastRod && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Resetting from cast...");
            animator.SetTrigger("resetFromCast");
            hasCastRod = false;
            isResettingFromCast = true;
            canCast = false;
        }

        // Start casting only if not resetting and allowed
        if (canCast && !hasCastRod && !isResettingFromCast)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Started charging rod...");
                animator.SetTrigger("startRodCharge");
                isThrowingRod = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Released rod cast!");
                animator.SetTrigger("releaseRodCast");
                isThrowingRod = false;
                hasCastRod = true;
                canCast = false;
            }
        }

        // Reset throwing flag if not holding
        if (!Input.GetMouseButton(0) && !hasCastRod)
        {
            isThrowingRod = false;
        }

        animator.SetBool("isThrowingRod", isThrowingRod);

        // Handle movement/idle
        isIdle = !isRunning && !isJumping;

        if (isThrowingRod)
        {
            animator.SetBool("isThrowingRunning", isRunning);
            animator.SetBool("isThrowingJumping", isJumping);
        }
        else
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isIdle", isIdle);
        }

        if (isJumping)
        {
            isJumping = false;
        }

        // ✅ Check if we've returned to Idle so we can cast again
        if (!canCast && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log("Back to idle, you can cast again!");
            FinishReset();
        }
    }

    public void FinishReset()
    {
        isResettingFromCast = false;
        canCast = true;
    }
}
