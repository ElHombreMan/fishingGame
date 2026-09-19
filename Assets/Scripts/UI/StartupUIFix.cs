using UnityEngine;

public class StartupUIFix : MonoBehaviour
{
    public GameObject[] panelsToHide;

    void Awake()
    {
        foreach (GameObject panel in panelsToHide)
        {
            if (panel != null)
            {
                panel.SetActive(false);
                Debug.Log("Disabled at runtime: " + panel.name);
            }
            else
            {
                Debug.LogWarning("UIInitializer: A panel is null!");
            }
        }
    }
}
