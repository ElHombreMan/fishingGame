using System.Collections;
using UnityEngine;

public class BlinkingTexture : MonoBehaviour
{
    [Header("Material Settings")]
    public Material openEyesMaterial;   // Material for eyes opened
    public Material closedEyesMaterial; // Material for eyes closed
    public float minBlinkTime = 0.1f;  // Minimum interval between blinks (eyes closing)
    public float maxBlinkTime = 0.2f;  // Maximum interval between blinks (eyes closing)
    public float timeEyesClosed = 0.05f; // Time the eyes stay closed (blink duration)

    private Renderer objectRenderer;

    void Start()
    {
        // Get the Renderer component from the object this script is attached to
        objectRenderer = GetComponent<Renderer>();

        // Start the blinking coroutine
        StartCoroutine(Blinking());
    }

    IEnumerator Blinking()
    {
        while (true)
        {
            // Wait for a random time before closing the eyes
            float waitTime = Random.Range(minBlinkTime, maxBlinkTime);
            yield return new WaitForSeconds(waitTime);

            // Change the material to closed eyes
            Material[] materials = objectRenderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = closedEyesMaterial;
            }
            objectRenderer.sharedMaterials = materials;

            // Wait for a very short time (blink duration) before reopening the eyes
            yield return new WaitForSeconds(timeEyesClosed);

            // Change the material to open eyes
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = openEyesMaterial;
            }
            objectRenderer.sharedMaterials = materials;

        }
    }
}
