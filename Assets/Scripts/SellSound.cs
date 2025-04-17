using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnClick : MonoBehaviour, IPointerClickHandler
{
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // This method is called automatically when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
