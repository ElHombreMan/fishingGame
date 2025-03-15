
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
    public GameObject fishingUI;
    public Image timeBar;
    public RectTransform playerLine;
    public RectTransform blueLine;
    public RectTransform whiteLine;

    [Header("Settings")]
    public float maxTime = 10f;
    public float requiredOverlapTime = 5f;
    public float blueMoveDuration = 1.5f;
    public float pauseMin = 0.5f;
    public float pauseMax = 1.5f;

    private float timeLeft;
    private bool gameActive = false;
    private float overlapTimer = 0f;

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
                StartMinigame();
            else
                EndMinigame();

        }

        if (gameActive)
        {
            // Countdown logic
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                LoseMinigame();
            }

            if (timeBar != null)
                timeBar.fillAmount = timeLeft / maxTime;

            Vector3 mousePos = Input.mousePosition;
            float clampedX = Mathf.Clamp(mousePos.x, whiteLineMinX, whiteLineMaxX);
            playerLine.position = new Vector3(clampedX, playerLine.position.y, playerLine.position.z);

            if (IsOverlapping(playerLine, blueLine))
            {
                overlapTimer += Time.deltaTime;
                if (overlapTimer >= requiredOverlapTime)
                {
                    GameSuccess();
                }
            }
            else
            {
                overlapTimer = 0f;
            }
        }
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
        overlapTimer = 0f;

        // Show the UI
        fishingUI.SetActive(true);

        // Store the current MaxSpeeds AND Values
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
        storedXValue = freeLookCam.m_XAxis.Value;
        storedYValue = freeLookCam.m_YAxis.Value;

        // Lock the camera by setting speeds to 0
        freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        freeLookCam.m_YAxis.m_MaxSpeed = 0f;

        // Unlock the mouse for UI usage
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reset the countdown
        timeLeft = maxTime;
        if (timeBar != null)
            timeBar.fillAmount = 1f;

        StartCoroutine(MoveBlueLineRoutine());
    }

    void EndMinigame()
    {
        gameActive = false;
        StopAllCoroutines();
        overlapTimer = 0f;

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
        Debug.Log("Success! You kept the line in the target area for 5 seconds");

    }

    void LoseMinigame()
    {
        EndMinigame();
        Debug.Log("You ran out of time and lost the fishing minigame!");
    }
}
