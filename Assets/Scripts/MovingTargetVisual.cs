using UnityEngine;
using UnityEngine.UI;

public class MovingTargetVisual : MonoBehaviour
{
    public Sprite commonSprite;
    public Sprite uncommonSprite;
    public Sprite rareSprite;
    public Sprite legendarySprite;

    public Image fishVisual;

    public void SetFishVisual(string rarity)
    {
        Debug.Log("Setting fish visual to: " + rarity);
        
        switch (rarity)
        {
            case "Common":
                fishVisual.sprite = commonSprite;
                break;
            case "Uncommon":
                fishVisual.sprite = uncommonSprite;
                break;
            case "Rare":
                fishVisual.sprite = rareSprite;
                break;
            case "Legendary":
                fishVisual.sprite = legendarySprite;
                break;
        }
    }
}
