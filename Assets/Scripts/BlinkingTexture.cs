using UnityEngine;
using System.Collections;

public class EyeBlink : MonoBehaviour
{
    public Material openEyesMaterial;  // Material for eyes opened
    public Material closedEyesMaterial; // Material for eyes closed
    public float blinkInterval = 2.0f; // Time between blinks
    public float blinkDuration = 0.1f; // How long the blink lasts

    private Renderer rend;

    void Start()
    {
        // Get the Renderer component
        rend = GetComponent<Renderer>();

        // Ensure the materials are assigned
        if (openEyesMaterial == null || closedEyesMaterial == null)
        {
            Debug.LogError("Eye materials not assigned!");
            return;
        }

        // Start the blinking coroutine
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Wait for the interval before the next blink
            yield return new WaitForSeconds(blinkInterval);

            // Start blink: close the eyes
            SetEyeMaterial(closedEyesMaterial);

            // Wait for the blink duration
            yield return new WaitForSeconds(blinkDuration);

            // Open the eyes after the blink
            SetEyeMaterial(openEyesMaterial);

            // Wait for the interval before the next blink
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void SetEyeMaterial(Material material)
    {
        // Change the material of the eye
        rend.material = material;
    }
}
