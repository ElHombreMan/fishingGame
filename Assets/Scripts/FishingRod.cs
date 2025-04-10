using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [Header("Bobber Settings")]
    public GameObject bobberPrefab;
    public Transform orientation;
    public Transform rodTip;
    public Vector3 spawnOffset = new Vector3(0, 0.3f, 0); // small offset UP
    public float maxCharge = 3f;
    public float throwForceMultiplier = 10f;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private GameObject currentBobber;

    void Update()
    {
        if (PlayerStateHandler.Instance.CurrentState == PlayerState.Idle)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCharging();
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Min(chargeTimer, maxCharge);
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ThrowBobber();
        }
    }

    void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0f;
        // TODO: Play charging animation
    }

    void ThrowBobber()
    {
        isCharging = false;
        // TODO: Play throw animation

        currentBobber = Instantiate(
            bobberPrefab,
            rodTip.position + spawnOffset,
            Quaternion.LookRotation(orientation.forward));

        Rigidbody rb = currentBobber.GetComponent<Rigidbody>();

        Vector3 throwDirection = orientation.forward;

        rb.AddForce(throwDirection * chargeTimer * throwForceMultiplier, ForceMode.Impulse);

        chargeTimer = 0f;
    }


    public void ReelBack()
    {
        if (currentBobber != null)
        {
            Destroy(currentBobber);
            currentBobber = null;
        }

        // TODO: Play reel animation
    }
}
