using UnityEngine;
using UnityEngine.UI;

public class ShopShipButton : MonoBehaviour
{
    public int price;
    public Sprite defaultSprite;
    public Sprite boughtSprite;
    public Inventory inventory;
    public GameObject prefabToSpawn; 
    public Transform spawnPoint;



    private bool isBought = false;
    private Image buttonImage;
    
    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        prefabToSpawn.SetActive(false);
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
            prefabToSpawn.SetActive(true);

            inventory.UpdateMoneyText();
            inventory.SaveInventory();

            Debug.Log("Ship bought!");
        }
    }

    void ShipIsBought()
    {
        void SpawnPrefab()
    {
        if (prefabToSpawn != null && spawnPoint != null)
        {
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }

    }
}