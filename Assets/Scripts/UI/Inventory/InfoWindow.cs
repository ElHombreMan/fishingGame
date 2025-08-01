using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoWindow : MonoBehaviour
{
    public TMP_Text nameText;
    public Image iconImage;
    public TMP_Text rarityText;
    public TMP_Text priceText;
    public TMP_Text sizeText;

    [Header("Back Ground Images")]
    public Image infoWindowReference;
    public Sprite commonBG;
    public Sprite legendaryBG;
    public Sprite uncommonBG;
    public Sprite rareBG;

    public void Show(FishData data, float length)
    {
        switch (data.rarity)
        {
            case "Common":
                infoWindowReference.sprite = commonBG;
                break;
            case "Uncommon":
                infoWindowReference.sprite = uncommonBG;
                break;
            case "Legendary":
                infoWindowReference.sprite = legendaryBG;
                break;
            case "Rare":
                infoWindowReference.sprite = rareBG;
                break;

        }
        nameText.text = data.fishName;
        iconImage.sprite = data.image;
        rarityText.text = "Rarity: " + data.rarity;
        priceText.text = "Price: $" + data.GetCost(length);
        sizeText.text = "Size: " + length.ToString("F2") + " in.";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

