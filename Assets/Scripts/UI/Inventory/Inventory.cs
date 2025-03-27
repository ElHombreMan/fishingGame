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
}
