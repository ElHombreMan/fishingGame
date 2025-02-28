using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingMinigame : MonoBehaviour
{
    [Header("UI Elements")]
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
        // Calculate the horizontal boundaries of the white line
        float halfWidth = (whiteLine.rect.width * whiteLine.lossyScale.x) / 2;
        whiteLineMinX = whiteLine.position.x - halfWidth;
        whiteLineMaxX = whiteLine.position.x + halfWidth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !gameActive)
        {
            StartMinigame();
        }

        if (gameActive)
        {
            // Update the player line's position
            Vector3 mousePos = Input.mousePosition;
            float clampedX = Mathf.Clamp(mousePos.x, whiteLineMinX, whiteLineMaxX);
            playerLine.position = new Vector3(clampedX, playerLine.position.y, playerLine.position.z);

            // Check if player line overlaps with blue line
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
        StartCoroutine(MoveBlueLineRoutine());
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
        
    }
}

