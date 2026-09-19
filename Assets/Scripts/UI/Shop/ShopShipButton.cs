using UnityEngine;
using UnityEngine.UI;

public class ShopShipButton : MonoBehaviour
{
    public int price;
    public Sprite defaultSprite;
    public Sprite boughtSprite;
    public Inventory inventory;
    public GameObject ShipThatEndsTheGame;
    public int dialogueBuyLineID;

    private bool isBought = false;
    private Image buttonImage;
    private ShopDialogue dialogue;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        dialogue = FindObjectOfType<ShopDialogue>(); // Find the dialogue system
    }

    void Start()
    {
        ShipThatEndsTheGame.SetActive(false);
        buttonImage.sprite = inventory.boughtShip ? boughtSprite : defaultSprite;
        isBought = inventory.boughtShip;

        // If the ship was already bought, re-enable it on scene load
        if (isBought && ShipThatEndsTheGame != null)
        {
            ShipThatEndsTheGame.SetActive(true);
        }
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
            inventory.UpdateMoneyText();
            inventory.SaveInventory();

            // Enable the ship in the scene
            if (ShipThatEndsTheGame != null)
                ShipThatEndsTheGame.SetActive(true);

            // Play dialogue line
            if (dialogue != null)
                dialogue.PlayLine(dialogueBuyLineID);

            Debug.Log("Ship bought!");
        }
    }
}
