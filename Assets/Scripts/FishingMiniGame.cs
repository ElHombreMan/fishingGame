
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingMinigame : MonoBehaviour
{
    [Header("References to Player Scripts")]
    public PlayerMovement playerMovement;   
    public PlayerCam playerCam;                    
    

    [Header("UI Elements")]
    public GameObject fishingUI;
    public RectTransform whiteLine;
    public RectTransform blueLine;
    public RectTransform playerLine;

    [Header("Game Settings")]
    public float requiredOverlapTime = 5f;
    public float blueMoveDuration = 1.5f;
    public float pauseMin = 0.5f;
    public float pauseMax = 1.5f;

    private bool gameActive = false;
    private float overlapTimer = 0f;

    private float whiteLineMinX, whiteLineMaxX;

    void Start()
    {
        fishingUI.SetActive(false);

        float halfWidth = (whiteLine.rect.width * whiteLine.lossyScale.x) / 2;
        whiteLineMinX = whiteLine.position.x - halfWidth;
        whiteLineMaxX = whiteLine.position.x + halfWidth;
    }

    void Update()
    {
        // Press F to toggle the minigame on/off
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!gameActive)
                StartMinigame();
            else
                EndMinigame(); 
        }

        if (gameActive)
        {
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
        gameActive = true;
        overlapTimer = 0f;

        // Show the UI
        fishingUI.SetActive(true);

        // Disable player movement and camera
        if (playerMovement) playerMovement.enabled = false;
        if (playerCam) playerCam.enabled = false;
        

        // Unlock the mouse for UI usage
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(MoveBlueLineRoutine());
    }

    void EndMinigame()
    {
        gameActive = false;
        StopAllCoroutines();
        overlapTimer = 0f;

        // Hide the UI
        fishingUI.SetActive(false);

        // Re-enable movement and camera
        if (playerMovement) playerMovement.enabled = true;
        if (playerCam) playerCam.enabled = true;

        // Lock the mouse again
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

    void GameSuccess()
    {
        gameActive = false;
        StopAllCoroutines();
        Debug.Log("Success! You kept the line in the target area for 5 seconds");

        
        if (playerMovement) playerMovement.enabled = true;
        if (playerCam) playerCam.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
