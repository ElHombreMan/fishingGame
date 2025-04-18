using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaterTeleport : MonoBehaviour
{
    public Transform player;
    public Vector3 teleportPosition = new Vector3(302.08f, 13.26f, -592.87f);
    public Image fadeImage; // Assign a black UI Image here
    public float fadeDuration = 1f;

    private bool isFading = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(TeleportWithFade());
        }
    }

    IEnumerator TeleportWithFade()
    {
        isFading = true;

        // Make sure the image is visible
        fadeImage.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeImage(0, 1));

        // Teleport player
        player.position = teleportPosition;

        // Fade out
        yield return StartCoroutine(FadeImage(1, 0));

        // Hide the image again
        fadeImage.gameObject.SetActive(false);

        isFading = false;
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
