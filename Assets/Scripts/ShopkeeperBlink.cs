using UnityEngine;

public class BlinkingMaterial : MonoBehaviour
{
    public Renderer targetRenderer;
    public Material normalMaterial;
    public Material blinkMaterial;

    public float minBlinkInterval = 1.5f;
    public float maxBlinkInterval = 3.0f;
    public float blinkDuration = 0.2f;

    private float nextBlinkTime;
    private float blinkTimer = 0f;
    private bool isBlinking = false;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        targetRenderer.material = normalMaterial;
        nextBlinkTime = GetRandomBlinkInterval();
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;

        if (!isBlinking && blinkTimer >= nextBlinkTime)
        {
            StartCoroutine(Blink());
        }
    }

    System.Collections.IEnumerator Blink()
    {
        isBlinking = true;
        targetRenderer.material = blinkMaterial;

        yield return new WaitForSeconds(blinkDuration);

        targetRenderer.material = normalMaterial;
        blinkTimer = 0f;
        nextBlinkTime = GetRandomBlinkInterval();
        isBlinking = false;
    }

    float GetRandomBlinkInterval()
    {
        return Random.Range(minBlinkInterval, maxBlinkInterval);
    }
}
