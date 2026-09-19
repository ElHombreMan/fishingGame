using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "FishingAlone/Fish")]
public class FishData : ScriptableObject
{
    public string fishName;
    public float minLength;
    public float maxLength;
    public int basePrice;
    public string rarity;
    [TextArea] public string description;
    public Sprite image;
    public Sprite icon;

    // Get a random length within the min/max range
    public float GetRandomLength()
    {
        return Random.Range(minLength, maxLength);
    }

    // Calculate fish cost dynamically based on length (exponential scaling)
    public int GetCost(float fishLength)
    {
        if (fishLength < minLength) fishLength = minLength;
        if (fishLength > maxLength) fishLength = maxLength;

        float k = 1.0f; // Scaling factor
        float p = 1.2f; // Power factor for non-linear scaling

        float percentage = (fishLength - minLength) / (maxLength - minLength);

        // Apply exponential scaling
        float multiplier = (1 + k * percentage);
        float lengthPrice = basePrice * (Mathf.Pow(multiplier, p) - 1); // Ensure base price is respected

        return Mathf.RoundToInt(basePrice + lengthPrice);
    }
}
