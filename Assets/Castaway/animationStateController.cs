using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    private bool isJumping;
    private bool isResettingFromCast = false;
    private bool canCast = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerStateHandler.Instance.CurrentState == PlayerState.InMiniGame ||
            PlayerStateHandler.Instance.CurrentState == PlayerState.InInventory ||
            PlayerStateHandler.Instance.CurrentState == PlayerState.InEscapeMenu)
            return;

        HandleMovementAnimations();
    }

    void HandleMovementAnimations()
    {
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.Space))
            isJumping = true;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isIdle", !isRunning && !isJumping);

        if (isJumping)
            isJumping = false;

        if (!canCast && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            FinishReset();
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

    public void FinishReset()
    {
        isResettingFromCast = false;
        canCast = true;
    }
}
