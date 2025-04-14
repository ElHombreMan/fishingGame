using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots;
    public InfoWindow infoWindow;
    public FishData[] allFish;
    public int worms = 0;
    public int money = 0;

    [System.Serializable]
    private class InventorySaveData
    {
        public List<FishSaveData> fishList;
        public int worms;
        public int money;
    }
    void Start()
    {
        LoadInventory();
    }

    void OnApplicationQuit()
    {
        SaveInventory();
    }

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

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();
        data.fishList = new List<FishSaveData>();

        foreach (InventorySlot slot in slots)
        {
            if (slot.isOccupied)
            {
                FishSaveData fishData = new FishSaveData
                {
                    fishName = slot.fishData.fishName,
                    length = slot.caughtLength
                };
                data.fishList.Add(fishData);
            }
        }

        data.worms = worms;
        data.money = money;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("InventoryData")) return;

        string json = PlayerPrefs.GetString("InventoryData");
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        worms = data.worms;
        money = data.money;

        foreach (FishSaveData fishData in data.fishList)
        {
            FishData fish = allFish.FirstOrDefault(f => f.fishName == fishData.fishName);
            if (fish != null)
                AddFish(fish, fishData.length);
        }
    }
}
