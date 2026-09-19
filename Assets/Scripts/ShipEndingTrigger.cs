using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;
    public float waitBeforeReturn = 6f;

    public AudioSource backgroundMusic;
    public AudioSource ambienceMusic;
    public AudioSource endingMusic;

    private bool hasEnded = false;

    void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnded)
        {
            StartCoroutine(FadeInThenReturn());
        }
    }

    IEnumerator FadeInThenReturn()
    {
        hasEnded = true;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause ambient and background music
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (ambienceMusic != null) ambienceMusic.Stop();

        // Start ending music
        if (endingMusic != null) endingMusic.Play();

        // Activate image and fade in
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Wait before returning
        yield return new WaitForSeconds(waitBeforeReturn);

        SceneManager.LoadScene("Menu"); // Replace with your actual menu scene name
    }
}
