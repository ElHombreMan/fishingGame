using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    private FishData fishData;
    private float caughtLength;
    private InfoWindow infoWindow;

    public bool isOccupied = false;

    public void SetFish(FishData data, float length, InfoWindow infoWin)
    {
        fishData = data;
        caughtLength = length;
        infoWindow = infoWin;
        isOccupied = true;

        icon.sprite = data.icon;
        icon.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOccupied && infoWindow != null)
            infoWindow.Show(fishData, caughtLength);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoWindow != null)
            infoWindow.Hide();
    }
}