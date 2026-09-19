using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public float delay = 0.05f; // Time between each character
    public AudioSource audioSource;

    private Coroutine typingCoroutine;
    private string currentFullText;

    public void StartTyping(string fullText, AudioClip sound = null)
    {
        StopDialogue(); // Stop any existing effect
        currentFullText = fullText;

        if (sound != null && audioSource != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }

        textBox.text = ""; // clear previous text
        typingCoroutine = StartCoroutine(TypeText(currentFullText));
    }

    private IEnumerator TypeText(string text)
    {
        foreach (char letter in text)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    public void StopDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        textBox.text = currentFullText; // instantly show remaining text
    }
}