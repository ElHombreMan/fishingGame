using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Inventory : MonoBehaviour

{
    [Header("Inventory")]
    public InventorySlot[] slots;
    public InfoWindow infoWindow;
    public FishData[] allFish;
    public TextMeshProUGUI inventoryMoneyText;

    public int money = 0;

    [Header("Shop")]
    public TextMeshProUGUI shopMoneyText;
    public List<RodType> boughtRods = new List<RodType>();
    public RodType selectedRod;
    public bool boughtShip = false;

    [Header("Debug Tools")]
    public int addMoneyAmount = 0;
    public bool addMoneyInEditor = false;
    public bool resetShopInEditor = false;
    private bool hasSoldFish = false;



    [System.Serializable]
    private class InventorySaveData
    {
        public List<FishSaveData> fishList;
        public int money;
        public List<string> boughtRods;
        public string selectedRod;
        public bool boughtShip;
    }
    void Start()
    {
        LoadInventory();
    }

    void OnApplicationQuit()
    {
        SaveInventory();
    }

    // just for testing, must be deleted later
    void Update()
    {
        if (resetShopInEditor) // only after reload
        {
            resetShopInEditor = false;
            hasSoldFish = false;
            ResetShop();
        }

        if (addMoneyInEditor)
        {
            addMoneyInEditor = false;
            money += addMoneyAmount;
            UpdateMoneyText();
            SaveInventory();
            Debug.Log("Added " + addMoneyAmount + " money.");
        }
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

    public void SellAllFish()
    {
        int totalMoney = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.isOccupied)
            {
                totalMoney += slot.fishData.GetCost(slot.caughtLength);
                slot.icon.enabled = false;
                slot.isOccupied = false;
            }
        }

        money += totalMoney;
        SaveInventory(); // optional, to instantly save
        UpdateMoneyText();

        Debug.Log("Sold all fish for $" + totalMoney);
                    ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();

            if (dialogue != null)
            {
                if (hasSoldFish == false)
                {
                    dialogue.PlayLineByID("FirstSell");
                    hasSoldFish = true;
                }
                else
                {
                string[] possibleIDs = { "Sell1", "Sell2", "Sell3" };
                string selectedID = possibleIDs[Random.Range(0, possibleIDs.Length)];
                Debug.Log("Chosen dialogue ID: " + selectedID);
                dialogue.PlayLineByID(selectedID);

                }
            }
        }

 


    public void UpdateMoneyText()
    {
        if (shopMoneyText != null)
            shopMoneyText.text = money.ToString();

        if (inventoryMoneyText != null)
            inventoryMoneyText.text = money.ToString();
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

        data.money = money;

        data.boughtRods = boughtRods.Select(r => r.ToString()).ToList();
        data.selectedRod = selectedRod.ToString();
        data.boughtShip = boughtShip;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("InventoryData")) return;

        string json = PlayerPrefs.GetString("InventoryData");
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        money = data.money;

        boughtRods = data.boughtRods.Select(r => (RodType)System.Enum.Parse(typeof(RodType), r)).ToList();
        selectedRod = (RodType)System.Enum.Parse(typeof(RodType), data.selectedRod);
        boughtShip = data.boughtShip;

        foreach (FishSaveData fishData in data.fishList)
        {
            FishData fish = allFish.FirstOrDefault(f => f.fishName == fishData.fishName);
            if (fish != null)
                AddFish(fish, fishData.length);
        }

        UpdateMoneyText();
    }
    public void ResetShop()
    {
        boughtRods.Clear(); 
        selectedRod = RodType.Basic; // Wooden rod is always bought + selected
        boughtRods.Add(RodType.Basic);
        boughtShip = false;

        SaveInventory();
    }
}


