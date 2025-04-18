using UnityEngine;
using UnityEngine.UI;

public class ShopRodButton : MonoBehaviour
{
    public RodType rodType;
    public int price;

    public Sprite defaultSprite;
    public Sprite boughtSprite;
    public Sprite selectedSprite;
    public int dialogueBuyLineID;
    public int dialogueEquipLineID;
    private ShopDialogue dialogue;

    [Header("Audio")]
    public AudioSource buySound;  // Sound for buying
    public AudioSource equipSound;  // Sound for equipping

    public Inventory inventory;
    public FishingRodController rodController;
    private bool isBought = false;

    private Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    // Auto setup sprites correctly
    void Start()
    {
        if (inventory.boughtRods.Contains(rodType))
        {
            isBought = true;
            GetComponent<Image>().sprite = inventory.selectedRod == rodType ? selectedSprite : boughtSprite;
        }
        else
        {
            GetComponent<Image>().sprite = defaultSprite;
        }
    }

    public void Setup(Inventory inv, FishingRodController rod)
    {
        inventory = inv;
        rodController = rod;

        if (inventory.boughtRods.Contains(rodType))
        {
            isBought = true;
            GetComponent<Image>().sprite = inventory.selectedRod == rodType ? selectedSprite : boughtSprite;
        }
        else
        {
            GetComponent<Image>().sprite = defaultSprite;
        }
    }

    public void OnClick()
    {
        ShopDialogue dialogue = FindObjectOfType<ShopDialogue>();

        // Buying logic
        if (!isBought)
        {
            if (inventory.money >= price)
            {
                inventory.money -= price;
                isBought = true;
                inventory.boughtRods.Add(rodType);   // add to bought list
                GetComponent<Image>().sprite = boughtSprite;
                inventory.UpdateMoneyText();

                // 🎵 Play BUY sound
                if (buySound != null)
                {
                    buySound.Play();
                }

                // 🎤 Play BUY voice line
                if (dialogue != null)
                {
                    dialogue.PlayLine(dialogueBuyLineID);
                }
            }
        }
        else
        {
            // Equipping logic (even after buying or if already bought)
            rodController.currentRod = rodType;
            inventory.selectedRod = rodType;
            ResetAllButtons();
            GetComponent<Image>().sprite = selectedSprite;

            // 🎵 Play EQUIP sound
            if (equipSound != null)
            {
                equipSound.Play();
            }

            // 🎤 Play EQUIP voice line
            if (dialogue != null)
            {
                dialogue.PlayLine(dialogueEquipLineID);
            }
        }

        inventory.SaveInventory();
    }

    void ResetAllButtons()
    {
        foreach (ShopRodButton button in FindObjectsOfType<ShopRodButton>())
        {
            if (button != this && button.isBought)
            {
                button.GetComponent<Image>().sprite = button.boughtSprite;
            }
        }
    }
}
