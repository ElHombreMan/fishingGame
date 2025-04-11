using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform orientation;
    public Transform rodTip;
    public GameObject bobberPrefab;
    public FishingMinigame fishingMinigame;

    [Header("Sounds")]
    public AudioSource chargingSound;  // loop
    public AudioSource castSound;      // fly sound
    public AudioSource failSound;      // reset
    public AudioSource splashSound;    // bobber in water
    public AudioSource successSound;   // minigame win
    public AudioSource failureSound; // minigame fail 

    [Header("Settings")]
    public Vector3 bobberOffset = new Vector3(0, 0.3f, 0);
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

        Rigidbody rb = currentBobber.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * chargeTimer * throwForceMultiplier, ForceMode.Impulse);

        chargeTimer = 0f;
    }

    void ResetSilent()
    {
        isCharging = false;
        chargingSound.Stop();
        chargeTimer = 0f;
        // No animation, no sound
    }

    public void ReelBack()
    {
        if (currentBobber)
        {
            Destroy(currentBobber);
            currentBobber = null;
        }

        ResetAllTriggers();

        animator.SetTrigger("resetFromCast");
        failSound.Play();
    }

    public void OnBobberLandedOnWater()
    {
        splashSound.Play();
        fishingMinigame.StartCoroutine(fishingMinigame.DelayedStart(1f));
    }

    public void OnMinigameSuccess()
    {
        successSound.Play();
        ReelBack();
    }

    public void OnMinigameFail()
    {
        failureSound.Play();
        ReelBack();
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("startRodCharge");
        animator.ResetTrigger("releaseRodCast");
        animator.ResetTrigger("resetFromCast");
    }
}
