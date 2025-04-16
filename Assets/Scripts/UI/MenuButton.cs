using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // <-- Import TextMeshPro namespace

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textComponent; // <-- Changed from Text to TextMeshProUGUI
    public Image imageComponent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        textComponent.color = Color.white;
        imageComponent.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textComponent.color = Color.black;
        imageComponent.enabled = false;
    }
}
