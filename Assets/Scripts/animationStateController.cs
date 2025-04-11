using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    private bool isJumping;
    private bool isResettingFromCast = false;
    private bool canCast = true;
    private bool grounded;

    public PlayerMovement playerMovement;
    public Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        if (rb == null)
            rb = playerMovement.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!CanMove())
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", true);
        }
        else
        {
            HandleMovementAnimations();
        } 
    }

    void HandleMovementAnimations()
    {
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        grounded = playerMovement.IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            isJumping = true;
            animator.SetTrigger("Jump");  // Play jump animation once
        }

        if (isJumping && grounded && rb.velocity.y <= 0.1f)
            isJumping = false;

        //animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning && !isJumping && grounded);
        animator.SetBool("isIdle", !isRunning && !isJumping);
    }

    public void TriggerStartCharge()
    {
        animator.SetTrigger("startRodCharge");
    }

    public void TriggerReleaseCast()
    {
        animator.SetTrigger("releaseRodCast");
    }

    public void TriggerResetCast()
    {
        animator.SetTrigger("resetFromCast");
    }

    bool CanMove()
    {
        return PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InMiniGame &&
            PlayerStateHandler.Instance.CurrentState != PlayerState.RodCharging;
    }

    public void FinishReset()
    {
        isResettingFromCast = false;
        canCast = true;
    }
}
