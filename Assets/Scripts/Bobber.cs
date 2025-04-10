using UnityEngine;

public class Bobber : MonoBehaviour
{
    private bool hasLanded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (collision.gameObject.CompareTag("Water"))
        {
            hasLanded = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            // Start Minigame
            FishingMinigame minigame = FindObjectOfType<FishingMinigame>();

            if (minigame != null)
            {
                StartCoroutine(minigame.DelayedStart(1f));
            }
        }
        else
        {
            // Hit anything except water ? destroy bobber
            Destroy(gameObject);
        }
    }
}
