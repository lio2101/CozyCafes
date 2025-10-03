using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private DropDownController screenSettings;
    [SerializeField] private DropDownController resolutions;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectVolume;
    [SerializeField] private Toggle babbleToggle;

    [SerializeField] private Button saveSettings;

    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        //fill screen setting options
        screenSettings.SetOptions(SettingsManager.WINDOW_MODE_NAMES);

        //fill resolution options

        List<string> resolutionList = new List<string>();
        foreach (Resolution resolution in SettingsManager.Instance.AvailableResolutions)
        {
            resolutionList.Add($"{resolution.width}x{resolution.height}");
        }
        resolutions.SetOptions(resolutionList);

    }

    private void OnEnable()
    {
        saveSettings.onClick.AddListener(SettingsManager.Instance.ApplySettings);
        EventSystem.current.SetSelectedGameObject(resolutions.gameObject);
        //resolutions.EnableHighlight();

        // Load current settings into the UI
        if (SettingsManager.Settings != null)
        {
            int resolutionIndex = Array.IndexOf(SettingsManager.SUPPORTED_WINDOW_MODES, SettingsManager.Settings.fullScreenMode);
            resolutions.SetValue(resolutionIndex);

            int screenModeIndex = Array.IndexOf(SettingsManager.SUPPORTED_WINDOW_MODES, SettingsManager.Settings.fullScreenMode);
            screenSettings.SetValue(screenModeIndex);

            masterVolume.value = SettingsManager.Settings.masterVolume;
            musicVolume.value = SettingsManager.Settings.musicVolume;
            effectVolume.value = SettingsManager.Settings.sfxVolume;
            babbleToggle.isOn = SettingsManager.Settings.babble;
        }
        //resolutions.onValueChanged.AddListener(SettingsManager.Instance.SetNewResolution);
        //screenSettings.onValueChanged.AddListener(SettingsManager.Instance.SetScreenSetting);

        resolutions.ValueChanged += SettingsManager.Instance.SetNewResolution;
        screenSettings.ValueChanged += SettingsManager.Instance.SetScreenSetting;

        masterVolume.onValueChanged.AddListener(SettingsManager.Instance.SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
        effectVolume.onValueChanged.AddListener(SettingsManager.Instance.SetEffectVolume);

        babbleToggle.onValueChanged.AddListener(SettingsManager.Instance.SetBabbleBool);
    }
    private void OnDisable()
    {
        saveSettings.onClick.RemoveListener(SettingsManager.Instance.ApplySettings);

        resolutions.ValueChanged -= SettingsManager.Instance.SetNewResolution;
        screenSettings.ValueChanged -= SettingsManager.Instance.SetScreenSetting;

        masterVolume.onValueChanged.RemoveListener(SettingsManager.Instance.SetMasterVolume);
        musicVolume.onValueChanged.RemoveListener(SettingsManager.Instance.SetMusicVolume);
        effectVolume.onValueChanged.RemoveListener(SettingsManager.Instance.SetEffectVolume);

        babbleToggle.onValueChanged.RemoveListener(SettingsManager.Instance.SetBabbleBool);

        SettingsManager.Instance.SaveSettings();
    }


}
