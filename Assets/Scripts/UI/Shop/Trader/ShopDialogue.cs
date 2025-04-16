using UnityEngine;

public class ShopDialogue : MonoBehaviour
{
    public DialogueLine[] lines;
    public TypewriterEffect typewriter;

    public void PlayLine(int index)
    {
        if (index < 0 || index >= lines.Length || typewriter == null) return;

        // Stop all voice sources before starting a new one
        foreach (DialogueLine line in lines)
        {
            if (line.voiceSource != null && line.voiceSource.isPlaying)
                line.voiceSource.Stop();
        }

        // Stop previous dialogue text
        typewriter.StopDialogue();

        // Start new dialogue
        DialogueLine selectedLine = lines[index];
        typewriter.StartTyping(selectedLine.text);

        if (selectedLine.voiceSource != null)
            selectedLine.voiceSource.Play();
    }

    public void StopLine()
    {
        if (typewriter != null)
            typewriter.StopDialogue();

        foreach (DialogueLine line in lines)
        {
            if (line.voiceSource != null && line.voiceSource.isPlaying)
                line.voiceSource.Stop();
        }
    }
}