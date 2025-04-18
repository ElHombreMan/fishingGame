using UnityEngine;

public class Bobber : MonoBehaviour
{
    private bool hasLanded = false;
    private FishingRodController rod;
    private Animator animator;

    public void Setup(FishingRodController fishingRod)
    {
        //Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red, 5f);
        rod = fishingRod;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bobber hit: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

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
            ResetAllTriggers();
            rod.animator.SetTrigger("cancelCast");
            rod.animator.SetTrigger("ResetFromCast");
        }
     }

    private void ResetAllTriggers()
    {
        rod.animator.ResetTrigger("startRodCharge");
        rod.animator.ResetTrigger("releaseRodCast");
        rod.animator.ResetTrigger("resetFromCast");
    }

}

