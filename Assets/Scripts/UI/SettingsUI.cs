using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityDisplay;

    void Start()
    {
        sensitivitySlider.wholeNumbers = true; //Only integers
        sensitivitySlider.minValue = 1;
        sensitivitySlider.maxValue = 20;

        sensitivitySlider.value = GlobalSettings.sensitivity;
        sensitivityDisplay.text = GlobalSettings.sensitivity.ToString();

        sensitivitySlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        GlobalSettings.sensitivity = intValue;
        sensitivityDisplay.text = intValue.ToString();
    }
}
