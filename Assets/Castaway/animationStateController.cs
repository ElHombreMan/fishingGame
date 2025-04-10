using System.Collections;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    public bool isJumping;
    public bool isThrowingRod;
    private bool isIdle;

    private bool hasCastRod = false;
    private bool isResettingFromCast = false;
    private bool canCast = true;

    // 🎣 Rod sound references
    public AudioSource chargingAudioSource;
    public AudioSource castAudioSource;
    public AudioSource pullBackAudioSource;

    // 💨 Jump dust effect
    public GameObject jumpDustEffectPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (chargingAudioSource == null) Debug.LogWarning("Charging AudioSource is not assigned.");
        if (castAudioSource == null) Debug.LogWarning("Cast AudioSource is not assigned.");
        if (pullBackAudioSource == null) Debug.LogWarning("Pull Back AudioSource is not assigned.");
    }

    void Update()
    {
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // 🟩 Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;

            // 💨 Spawn jump smoke effect
            if (jumpDustEffectPrefab != null)
            {
                Vector3 spawnPosition = transform.position + Vector3.up * 0.35f;
                Instantiate(jumpDustEffectPrefab, spawnPosition, Quaternion.identity);
            }
        }

        // 🐟 Reset cast state
        if (hasCastRod && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Resetting from cast...");
            animator.SetTrigger("resetFromCast");
            hasCastRod = false;
            isResettingFromCast = true;
            canCast = false;

            if (pullBackAudioSource)
                pullBackAudioSource.Play();
        }

        if (canCast && !hasCastRod && !isResettingFromCast)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Started charging rod...");
                animator.SetTrigger("startRodCharge");
                isThrowingRod = true;

                if (chargingAudioSource && !chargingAudioSource.isPlaying)
                    chargingAudioSource.PlayOneShot(chargingAudioSource.clip);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Released rod cast!");
                animator.SetTrigger("releaseRodCast");
                isThrowingRod = false;
                hasCastRod = true;
                canCast = false;

                if (chargingAudioSource && chargingAudioSource.isPlaying)
                    chargingAudioSource.Stop();

                if (castAudioSource)
                    castAudioSource.Play();
            }
        }

        if (!Input.GetMouseButton(0) && !hasCastRod)
        {
            isThrowingRod = false;
        }

        animator.SetBool("isThrowingRod", isThrowingRod);

        isIdle = !isRunning && !isJumping;

        if (!isThrowingRod)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isIdle", isIdle);
        }

        if (isJumping)
        {
            isJumping = false;
        }

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
