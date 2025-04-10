using UnityEngine;

public class Bobber : MonoBehaviour
{
    private bool hasLanded = false;
    private FishingRodController rod;

    public void Setup(FishingRodController fishingRod)
    {
        rod = fishingRod;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;
        hasLanded = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (collision.gameObject.CompareTag("Water"))
        {
            rod.OnBobberLandedOnWater();
        }
        else
        {
            rod.ReelBack();
        }
    }
}

