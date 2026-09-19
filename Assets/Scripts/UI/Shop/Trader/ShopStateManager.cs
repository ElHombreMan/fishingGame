using UnityEngine;
using System.IO;

public class ShopStateManager : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "shopstate.json");

    [System.Serializable]
    public class ShopData
    {
        public bool hasOpenedShop = false;
    }

    public static ShopData Current = new ShopData();

    [Header("Debug Tools")]
    public bool resetShopOpenedInEditor = false;

    void Awake()
    {
        Load();
        ShopDebug();
    }

    private void ShopDebug()
    {
        if (resetShopOpenedInEditor)
        {
            Current.hasOpenedShop = false;
            Save();

            Debug.Log("<color=yellow>[DEBUG]</color> ShopStateManager: hasOpenedShop reset to FALSE");
        }
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(Current, true);
        File.WriteAllText(SavePath, json);
    }

    public static void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            Current = JsonUtility.FromJson<ShopData>(json);
        }
    }
}