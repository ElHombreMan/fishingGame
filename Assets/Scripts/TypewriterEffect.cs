using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public float delay = 0.05f; // Time between each character

    private Coroutine typingCoroutine;

    public void StartTyping(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string text)
    {
        textBox.text = "";
        foreach (char letter in text)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}
