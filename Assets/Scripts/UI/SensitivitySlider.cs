using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    public SettingsData settingsData; 

    void Start()
    {

        float savedSensitivity = settingsData.sensitivity;
        sensitivitySlider.value = savedSensitivity;

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    public void OnSensitivityChanged(float value)
    {
        settingsData.sensitivity = value;
    }
}
