using UnityEngine;

public class RodMaterialChanger : MonoBehaviour
{
    [Header("Rod Materials")]
    public Material basicMaterial;
    public Material betterMaterial;
    public Material bestMaterial;

    [Header("Rod Reference")]
    public MeshRenderer rodRenderer;

    [Header("Chances References")]
    public FishingRodController rodController;

    private RodChances lastChances;

    void Update()
    {
        RodChances currentChances = GetCurrentChances();

        if (currentChances != lastChances)
        {
            UpdateRodMaterial(currentChances);
            lastChances = currentChances;
        }
    }

    RodChances GetCurrentChances()
    {
        switch (rodController.currentRod)
        {
            case RodType.Basic:
                return rodController.basicRodChances;
            case RodType.Better:
                return rodController.betterRodChances;
            case RodType.Best:
                return rodController.bestRodChances;
            default:
                return rodController.basicRodChances;
        }
    }

    void UpdateRodMaterial(RodChances chances)
    {
        if (chances == rodController.basicRodChances)
            rodRenderer.material = basicMaterial;
        else if (chances == rodController.betterRodChances)
            rodRenderer.material = betterMaterial;
        else if (chances == rodController.bestRodChances)
            rodRenderer.material = bestMaterial;
    }
}
