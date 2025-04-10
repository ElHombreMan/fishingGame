using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;

public class FishingMinigame : MonoBehaviour
{
    [Header("Player References")]
    public PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;

    [Header("Cameras")]
    public CinemachineFreeLook freeLookCam;
    private float storedXMaxSpeed;
    private float storedYMaxSpeed;
    private float storedXValue;
    private float storedYValue;

    [Header("UI References")]
    public Inventory inventory;
    public GameObject fishingUI;
    public Image timeBar;
    public RectTransform playerLine;
    public RectTransform blueLine;
    public RectTransform whiteLine;

    [Header("Settings")]
    public float fillingSpeed = 0.7f;
    public float maxTime = 10f;
    public float blueMoveDuration = 1.5f;
    public float pauseMin = 0.5f;
    public float pauseMax = 1f;

    private bool canPlay = false;
    private float timeLeft;
    private bool gameActive = false;
    

    private float whiteLineMinX, whiteLineMaxX;

    void Start()
    {
        fishingUI.SetActive(false);

        float halfWidth = (whiteLine.rect.width * whiteLine.lossyScale.x) / 2;
        whiteLineMinX = whiteLine.position.x - halfWidth;
        whiteLineMaxX = whiteLine.position.x + halfWidth;

        if (timeBar != null)
            timeBar.fillAmount = 0f;
    }

    void Update()
    {
        // Press F to toggle the minigame on/off
        if (Input.GetKeyDown(KeyCode.F)
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory
            && PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu)
        {
            if (!gameActive)
                StartCoroutine(DelayedStart(1f)); // Delay start
            else
                EndMinigame();
        }

        if (gameActive)
        {
            // Always allow player line movement
            Vector3 mousePos = Input.mousePosition;
            float clampedX = Mathf.Clamp(mousePos.x, whiteLineMinX, whiteLineMaxX);
            playerLine.position = new Vector3(clampedX, playerLine.position.y, playerLine.position.z);

            if (!canPlay) return; // Don’t update time or anything else until after delay

            // Game is activestart updating time
            if (IsOverlapping(playerLine, blueLine))
            {
                timeLeft += Time.deltaTime * fillingSpeed; 
            }
            else
            {
                // Drain faster the closer you are to the top
                float drainMultiplier = Mathf.Lerp(1f, 3f, timeLeft / maxTime);
                timeLeft -= Time.deltaTime * drainMultiplier;
            }

            if (timeBar != null)
                timeBar.fillAmount = timeLeft / maxTime;

            if (timeLeft >= maxTime)
            {
                GameSuccess();
            }
            else if (timeLeft <= 0f)
            {
                LoseMinigame();
            }
        }
    }

    public IEnumerator DelayedStart(float delay)
    {
        StartMinigame(); 
        yield return new WaitForSeconds(delay);
        canPlay = true;                         
        StartCoroutine(MoveBlueLineRoutine());
    }

    bool IsOverlapping(RectTransform a, RectTransform b)
    {
        float aMin = a.position.x - (a.rect.width * a.lossyScale.x / 2);
        float aMax = a.position.x + (a.rect.width * a.lossyScale.x / 2);
        float bMin = b.position.x - (b.rect.width * b.lossyScale.x / 2);
        float bMax = b.position.x + (b.rect.width * b.lossyScale.x / 2);

        return aMax >= bMin && bMax >= aMin;
    }

    void StartMinigame()
    {
        PlayerStateHandler.Instance.ChangeState(PlayerState.InMiniGame);
        gameActive = true;
        canPlay = false; 
        
        fishingUI.SetActive(true);

        // Lock the camera
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
        storedXValue = freeLookCam.m_XAxis.Value;
        storedYValue = freeLookCam.m_YAxis.Value;

        freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        freeLookCam.m_YAxis.m_MaxSpeed = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Start bar at half
        timeLeft = maxTime / 2f;
        if (timeBar != null)
            timeBar.fillAmount = 0.5f;
    }

    void EndMinigame()
    {
        gameActive = false;
        StopAllCoroutines();
      

        // Hide the UI
        fishingUI.SetActive(false);

        // Restore axis Value
        freeLookCam.m_XAxis.Value = storedXValue;
        freeLookCam.m_YAxis.Value = storedYValue;

        // Restore original MaxSpeeds
        freeLookCam.m_XAxis.m_MaxSpeed = storedXMaxSpeed;
        freeLookCam.m_YAxis.m_MaxSpeed = storedYMaxSpeed;

        // Lock the mouse again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<FishingRod>().ReelBack();

        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
    }

    IEnumerator ReEnableCameraNextFrame()
    {
        yield return null; // wait one frame so orientation stays
        thirdPersonCam.enabled = true;
    }

    IEnumerator MoveBlueLineRoutine()
    {
        while (gameActive)
        {
            float blueHalfWidth = blueLine.rect.width * blueLine.lossyScale.x / 2;
            float targetX = Random.Range(whiteLineMinX + blueHalfWidth, whiteLineMaxX - blueHalfWidth);

            Vector3 startPos = blueLine.position;
            Vector3 targetPos = new Vector3(targetX, blueLine.position.y, blueLine.position.z);
            float elapsed = 0f;

            while (elapsed < blueMoveDuration)
            {
                blueLine.position = Vector3.Lerp(startPos, targetPos, elapsed / blueMoveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            blueLine.position = targetPos;

            yield return new WaitForSeconds(Random.Range(pauseMin, pauseMax));
        }
    }

    void GameSuccess()
    {
        EndMinigame();

        FishData randomFish = inventory.GetRandomFish();
        float length = randomFish.GetRandomLength();

        inventory.AddFish(randomFish, length);

        Debug.Log($"Caught a {randomFish.fishName} ({length:F2}m)");
    }

    void LoseMinigame()
    {
        EndMinigame();
        Debug.Log("You ran out of time and lost the fishing minigame!");
    }
}