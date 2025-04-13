using System.Collections;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform orientation;
    public Transform rodTip;
    public GameObject bobberPrefab;
    public FishingMinigame fishingMinigame;
    public LineRenderer fishingLine;

    [Header("Sounds")]
    public AudioSource chargingSound;
    public AudioSource castSound;
    public AudioSource failSound;
    public AudioSource splashSound;
    public AudioSource successSound;
    public AudioSource failureSound;


    [Header("Settings")]
    public Vector3 bobberOffset = new Vector3(0, 2f, 0);
    public float maxCharge = 3f;
    public float throwForceMultiplier = 10f;
    public float minChargeToThrow = 0.3f;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private GameObject currentBobber;

    void Update()
    {
        if (!CanFish()) return;

        if (Input.GetMouseButtonDown(0) && currentBobber == null)
        {
            chargeTimer = 0f;
            isCharging = true;
            PlayerStateHandler.Instance.ChangeState(PlayerState.RodCharging);
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Min(chargeTimer, maxCharge);

            if (chargeTimer >= minChargeToThrow && !chargingSound.isPlaying)
            {
                chargingSound.Play();
                animator.SetTrigger("startRodCharge");
            }
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            if (chargeTimer >= minChargeToThrow)
            {
                ThrowBobber();
            }
            else
            {
                ResetSilent();
            }
        }

        if (currentBobber)
        {
            fishingLine.SetPosition(0, rodTip.position);
            fishingLine.SetPosition(1, currentBobber.transform.position);
        }
    }

    bool CanFish()
    {
        return PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InMiniGame;
    }

    void ThrowBobber()
    {
        isCharging = false;
        chargingSound.Stop();

        animator.SetTrigger("releaseRodCast");
        castSound.Play();

        currentBobber = Instantiate(bobberPrefab, rodTip.position + bobberOffset, Quaternion.LookRotation(orientation.forward));
        currentBobber.GetComponent<Bobber>().Setup(this);

        fishingLine.enabled = true;
        fishingLine.positionCount = 2;

        Rigidbody rb = currentBobber.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * chargeTimer * throwForceMultiplier, ForceMode.Impulse);

        chargeTimer = 0f;
    }

    void ResetSilent()
    {
        isCharging = false;
        chargingSound.Stop();
        chargeTimer = 0f;
        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
    }

    public void ReelBack()
    {
        if (currentBobber)
        {
            Destroy(currentBobber);
            currentBobber = null;
        }

        fishingLine.enabled = false;

        ResetAllTriggers();

        animator.SetTrigger("resetFromCast");
        failSound.Play();

        StopReeling(); // ⬅️ stop reeling animation

        StartCoroutine(DelayedIdleState());
    }

    private IEnumerator DelayedIdleState()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
    }

    public void OnBobberLandedOnWater()
    {
        splashSound.Play();
        StartCoroutine(WaitBeforeMinigame());
    }

    private IEnumerator WaitBeforeMinigame()
    {
        float waitTime = Random.Range(5f, 20f);
        yield return new WaitForSeconds(waitTime);

        StartReeling(); // ⬅️ start reeling animation
        fishingMinigame.StartCoroutine(fishingMinigame.DelayedStart(0.7f));
    }

    public void OnMinigameSuccess()
    {
        successSound.Play();
        StopReeling(); // ⬅️ stop reeling animation
        ReelBack();
    }

    public void OnMinigameFail()
    {
        failureSound.Play();
        StopReeling(); // ⬅️ stop reeling animation
        ReelBack();
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("startRodCharge");
        animator.ResetTrigger("releaseRodCast");
        animator.ResetTrigger("resetFromCast");
    }

    // 🎣 Reeling control methods
    public void StartReeling()
    {
        animator.SetBool("isReeling", true);
    }

    public void StopReeling()
    {
        animator.SetBool("isReeling", false);
    }
}
