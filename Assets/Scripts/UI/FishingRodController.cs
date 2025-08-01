﻿using System.Collections;
using UnityEngine;

public enum RodType
{
    Basic,
    Better,
    Best
}

[System.Serializable]
public class RodChances
{
    public float commonChance;
    public float rareChance;
    public float epicChance;
    public float legendaryChance;
}

public class FishingRodController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform orientation;
    public Transform rodTip;
    public GameObject bobberPrefab;
    public FishingMinigame fishingMinigame;
    public LineRenderer fishingLine;
    public InventoryToggle inventoryToggle;
    [SerializeField] private MovingTargetVisual fishVisualController;



    [Header("Sounds")]
    public AudioSource chargingSound;
    public AudioSource castSound;
    public AudioSource failSound;
    public AudioSource splashSound;
    public AudioSource successSound;
    public AudioSource failureSound;
    public AudioSource reelingSound;


    [Header("Settings")]
    public Vector3 bobberOffset = new Vector3(0, 2f, 0);
    public float maxCharge = 3f;
    public float throwForceMultiplier = 10f;
    public float minChargeToThrow = 0.3f;

    [Header("Rarity Settings")]
    public RodType currentRod = RodType.Basic;
    public RodChances basicRodChances;
    public RodChances betterRodChances;
    public RodChances bestRodChances;

     [Header("UI")]
    [SerializeField] private ChargingMeter chargingMeter;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    public GameObject currentBobber;
    private Coroutine waitingForMinigameCoroutine;
    [HideInInspector] public FishData currentFish;

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
                chargingMeter.StartCharging();
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

            if (Input.GetMouseButtonDown(0))
            {
                ReelBack();
            }
        }
        // for testing delete later
        //HandleRodSwitching();
    }
    // for testing delete later
   /* void HandleRodSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentRod = RodType.Basic;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            currentRod = RodType.Better;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            currentRod = RodType.Best;
    }*/

    public string GetFishRarity()
    {
        RodChances chances = basicRodChances; // default

        switch (currentRod)
        {
            case RodType.Basic:
                chances = basicRodChances;
                break;
            case RodType.Better:
                chances = betterRodChances;
                break;
            case RodType.Best:
                chances = bestRodChances;
                break;
        }

        float rng = Random.Range(0f, 100f);

        if (rng <= chances.legendaryChance) return "Legendary";
        if (rng <= chances.legendaryChance + chances.epicChance) return "Rare";
        if (rng <= chances.legendaryChance + chances.epicChance + chances.rareChance) return "Uncommon";
        return "Common";
    }

    bool CanFish()
    {
        return PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InMiniGame
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InShop;
    }

    void ThrowBobber()
    {
        isCharging = false;
        chargingSound.Stop();

        if (waitingForMinigameCoroutine != null)
        {
            StopCoroutine(waitingForMinigameCoroutine);
            waitingForMinigameCoroutine = null;
        }

        animator.SetTrigger("releaseRodCast");
        castSound.Play();
        chargingMeter.StopCharging();


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
        if (waitingForMinigameCoroutine != null)
        {
            StopCoroutine(waitingForMinigameCoroutine);
            waitingForMinigameCoroutine = null;
        }

        if (currentBobber)
        {
            Destroy(currentBobber);
            currentBobber = null;
        }

        fishingLine.enabled = false;

        ResetAllTriggers();
        animator.SetTrigger("cancelCast");
        failSound.Play();

        StopReeling();

        StartCoroutine(DelayedIdleState());
    }

    private IEnumerator DelayedIdleState()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
        Debug.Log("Set player state to Idle");
    }

    public void OnBobberLandedOnWater()
    {
        splashSound.Play();
        string rarity = GetFishRarity();
        currentFish = fishingMinigame.inventory.GetRandomFishByRarity(rarity);
        waitingForMinigameCoroutine = StartCoroutine(WaitBeforeMinigame());
        if (fishVisualController != null)
        {
            fishVisualController.SetFishVisual(rarity);
        }
    }

    private IEnumerator WaitBeforeMinigame()
    {
        float waitTime = Random.Range(5f, 10f);
        yield return new WaitForSeconds(waitTime);

        if (inventoryToggle != null && inventoryToggle.inventoryPanel.activeSelf)
        {
            inventoryToggle.SendMessage("ToggleInventory"); // Calls ToggleInventory() from InventoryToggle.cs
        }

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
        if (!reelingSound.isPlaying)
            reelingSound.Play();
    }

    public void StopReeling()
    {
        animator.SetBool("isReeling", false);
        if (reelingSound.isPlaying)
            reelingSound.Stop();
    }
}
