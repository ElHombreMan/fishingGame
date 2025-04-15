using UnityEngine;
using UnityEngine.UI;

public class ChargingMeter : MonoBehaviour
{
    public GameObject chargingMeterUI;       // The whole UI container
    public Image fillImage;                  // The moving bar
    public float fillSpeed = 0.5f;           // How fast it fills

    private float currentFill = 0f;
    private bool isCharging = false;

    public void StartCharging()
    {
        isCharging = true;
        if (chargingMeterUI != null)
            chargingMeterUI.SetActive(true);
    }

    public void StopCharging()
    {
        isCharging = false;
        currentFill = 0f;

        if (chargingMeterUI != null)
            chargingMeterUI.SetActive(false);

        if (fillImage != null)
            fillImage.fillAmount = 0f;
    }

    void Update()
    {
        if (!isCharging) return;

        // Fill up only to 100% and stay there
        if (currentFill < 1f)
        {
            currentFill += fillSpeed * Time.deltaTime;
            currentFill = Mathf.Clamp01(currentFill);

            if (fillImage != null)
                fillImage.fillAmount = currentFill;
        }

        // Smooth color transition from Red → Yellow → Green
        if (fillImage != null)
        {
            if (currentFill < 1f)
            {
                // Blend red (1,0,0) to yellow (1,1,0)
                fillImage.color = Color.Lerp(Color.red, Color.yellow, currentFill);
            }
            else
            {
                // At full charge, snap to green
                fillImage.color = Color.green;
            }
        }
    }
}
