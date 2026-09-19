using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;

public class FishingMinigame : MonoBehaviour
{
    [Header("References")]
    public FishingRodController fishingRodController;
    public PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;
    public Inventory inventory;
    public GameObject fishingUI;
    public CinemachineFreeLook freeLookCam;

    [Header("UI")]
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
    private bool gameActive = false;
    private float timeLeft;
    private float whiteLineMinX, whiteLineMaxX;
    private float storedXMaxSpeed, storedYMaxSpeed, storedXValue, storedYValue;
    private string currentFishRarity;

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
        /*if (Input.GetKeyDown(KeyCode.F) && CanToggle())
        {
            if (!gameActive)
                StartCoroutine(DelayedStart(1f));
            else
                EndMinigame();
        }*/

        if (!gameActive)
            return;

        HandlePlayerLineMovement();

        if (!canPlay)
            return;

        HandleTimer();
    }

    public void SetDifficulty(string rarity)
    {
        switch (rarity)
        {
            case "Common":
                blueLine.sizeDelta = new Vector2(30f, blueLine.sizeDelta.y);
                blueMoveDuration = 2f;
                pauseMin = 0.7f;
                pauseMax = 0.9f;
                break;

            case "Uncommon":
                blueLine.sizeDelta = new Vector2(30f, blueLine.sizeDelta.y);
                blueMoveDuration = 1.5f;
                pauseMin = 0.5f;
                pauseMax = 0.7f;
                break;

            case "Rare":
                blueLine.sizeDelta = new Vector2(30f, blueLine.sizeDelta.y);
                blueMoveDuration = 1f;
                pauseMin = 0.3f;
                pauseMax = 0.5f;
                break;

            case "Legendary":
                blueLine.sizeDelta = new Vector2(30f, blueLine.sizeDelta.y);
                blueMoveDuration = 0.5f;
                pauseMin = 0.1f;
                pauseMax = 0.3f;
                break;
        }
    }

    bool CanToggle()
    {
        return PlayerStateHandler.Instance.CurrentState != PlayerState.InInventory &&
               PlayerStateHandler.Instance.CurrentState != PlayerState.InEscapeMenu &&
               PlayerStateHandler.Instance.CurrentState != PlayerState.InShop;
    }

    void HandlePlayerLineMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        float clampedX = Mathf.Clamp(mousePos.x, whiteLineMinX, whiteLineMaxX);
        playerLine.position = new Vector3(clampedX, playerLine.position.y, playerLine.position.z);
    }

    void HandleTimer()
    {
        if (IsOverlapping(playerLine, blueLine))
            timeLeft += Time.deltaTime * fillingSpeed;
        else
            timeLeft -= Time.deltaTime * Mathf.Lerp(1f, 3f, timeLeft / maxTime);

        if (timeBar != null)
            timeBar.fillAmount = timeLeft / maxTime;

        if (timeLeft >= maxTime)
            GameSuccess();
        else if (timeLeft <= 0f)
            LoseMinigame();
    }

    public IEnumerator DelayedStart(float delay)
    {
        StartMinigame();
        yield return new WaitForSeconds(delay);
        canPlay = true;
        StartCoroutine(MoveBlueLineRoutine());
    }

    void StartMinigame()
    {
        PlayerStateHandler.Instance.ChangeState(PlayerState.InMiniGame);
        gameActive = true;
        canPlay = false;
        fishingUI.SetActive(true);

        blueLine.position = whiteLine.position;

        StoreCameraState();
        LockCamera();
        ShowCursor();

        currentFishRarity = fishingRodController.currentFish.rarity;
        SetDifficulty(currentFishRarity);

        timeLeft = maxTime / 2f;
        if (timeBar != null)
            timeBar.fillAmount = 0.5f;
    }

    void EndMinigame()
    {
        gameActive = false;
        StopAllCoroutines();

        fishingUI.SetActive(false);

        RestoreCameraState();
        HideCursor();

        PlayerStateHandler.Instance.ChangeState(PlayerState.Idle);
    }

    void StoreCameraState()
    {
        storedXMaxSpeed = freeLookCam.m_XAxis.m_MaxSpeed;
        storedYMaxSpeed = freeLookCam.m_YAxis.m_MaxSpeed;
        storedXValue = freeLookCam.m_XAxis.Value;
        storedYValue = freeLookCam.m_YAxis.Value;
    }

    void LockCamera()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        freeLookCam.m_YAxis.m_MaxSpeed = 0f;
    }

    void RestoreCameraState()
    {
        freeLookCam.m_XAxis.Value = storedXValue;
        freeLookCam.m_YAxis.Value = storedYValue;

        freeLookCam.m_XAxis.m_MaxSpeed = storedXMaxSpeed;
        freeLookCam.m_YAxis.m_MaxSpeed = storedYMaxSpeed;
    }

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    bool IsOverlapping(RectTransform a, RectTransform b)
    {
        float aMin = a.position.x - (a.rect.width * a.lossyScale.x / 2);
        float aMax = a.position.x + (a.rect.width * a.lossyScale.x / 2);
        float bMin = b.position.x - (b.rect.width * b.lossyScale.x / 2);
        float bMax = b.position.x + (b.rect.width * b.lossyScale.x / 2);
        return aMax >= bMin && bMax >= aMin;
    }

    void GameSuccess()
    {
        EndMinigame();

        inventory.AddFish(fishingRodController.currentFish, fishingRodController.currentFish.GetRandomLength());

        fishingRodController.OnMinigameSuccess();
    }

    void LoseMinigame()
    {
        EndMinigame();
        fishingRodController.OnMinigameFail();
    }
}
