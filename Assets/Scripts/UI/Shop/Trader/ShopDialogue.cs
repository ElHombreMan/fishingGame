using UnityEngine;

public class ShopDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [Tooltip("Give this line an easy-to-remember ID (e.g. 'FirstSell', 'Sell1')")]
        public string lineID;

        [TextArea]
        public string text;

        public AudioSource voiceSource;
    }

    public DialogueLine[] lines;
    public TypewriterEffect typewriter;

    public void PlayLine(int index)
    {
        if (index < 0 || index >= lines.Length || typewriter == null) return;

        StopAllLines();

        DialogueLine selectedLine = lines[index];
        typewriter.StartTyping(selectedLine.text);

        if (selectedLine.voiceSource != null)
            selectedLine.voiceSource.Play();
    }

    public void PlayLineByID(string lineID)
    {
        Debug.Log("Attempting to play dialogue with ID: " + lineID);
        DialogueLine selectedLine = System.Array.Find(lines, line => line.lineID == lineID);
        if (selectedLine == null || typewriter == null)
        {
            Debug.LogWarning("Dialogue line ID not found: " + lineID);
            return;
        }

        StopAllLines();
        typewriter.StartTyping(selectedLine.text);

        if (selectedLine.voiceSource != null)
            selectedLine.voiceSource.Play();
    }

    public void StopLine()
    {
        if (typewriter != null)
            typewriter.StopDialogue();

        StopAllLines();
    }

    private void StopAllLines()
    {
        foreach (DialogueLine line in lines)
        {
            if (line.voiceSource != null && line.voiceSource.isPlaying)
                line.voiceSource.Stop();
        }
    }
}
