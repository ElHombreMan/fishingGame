using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots;
    public InfoWindow infoWindow;
    public FishData[] allFish;

    public void AddFish(FishData fish, float length)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.isOccupied)
            {
                slot.SetFish(fish, length, infoWindow);
                return;
            }
        }

        Debug.Log("Inventory Full!");
    }

    public FishData GetRandomFish()
    {
        int i = Random.Range(0, allFish.Length);
        return allFish[i];
    }

    public FishData GetRandomFishByRarity(string rarity)
    {
        List<FishData> possibleFish = new List<FishData>();

        foreach (FishData fish in allFish)
        {
            if (fish.rarity == rarity)
            {
                possibleFish.Add(fish);
            }
        }

        if (possibleFish.Count == 0)
        {
            Debug.LogWarning("No fish with rarity: " + rarity);
            return GetRandomFish(); // fallback to any fish
        }

        return possibleFish[Random.Range(0, possibleFish.Count)];
    }
}
