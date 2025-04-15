using UnityEngine;
using UnityEngine.UI;

public class ShopShipButton : MonoBehaviour
{
    public int price;
    public Sprite defaultSprite;
    public Sprite boughtSprite;
    public Inventory inventory;

    private bool isBought = false;
    private Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        buttonImage.sprite = inventory.boughtShip ? boughtSprite : defaultSprite;
        isBought = inventory.boughtShip;
    }

    public void OnClick()
    {
        if (isBought) return;

        if (inventory.money >= price)
        {
            inventory.money -= price;
            inventory.boughtShip = true;
            isBought = true;

            buttonImage.sprite = boughtSprite;

            ShipIsBought();

            inventory.UpdateMoneyText();
            inventory.SaveInventory();

            Debug.Log("Ship bought!");
        }
    }

    void ShipIsBought()
    {
        //end game logic
    }
}