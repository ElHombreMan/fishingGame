using UnityEngine;
using UnityEngine.UI;

public class FishHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill; // This is the image that gets filled
    [SerializeField] private Gradient healthColorGradient; // We'll use this to shift color

    private float currentHealth;
    private float maxHealth = 100f;

    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        float fillPercent = currentHealth / maxHealth;

        healthBarFill.fillAmount = fillPercent;
        healthBarFill.color = healthColorGradient.Evaluate(fillPercent);
    }

    // Optional: call this at the beginning to fully restore health
    public void ResetHealth()
    {
        SetHealth(maxHealth);
    }
}
