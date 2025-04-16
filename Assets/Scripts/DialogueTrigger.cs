using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TypewriterEffect typewriter;
    public AudioSource introSound;

    void Start()
    {
        // Play the introduction sound
        if (introSound != null)
        {
            introSound.Play();
        }

        // Start the typewriter dialogue
        if (typewriter != null)
        {
            typewriter.StartTyping("Welcome to my shop. Please feed me some fish I am so very hungry. I promise to give you something in return.");
        }
    }
}
