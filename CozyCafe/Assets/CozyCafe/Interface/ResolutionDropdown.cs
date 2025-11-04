using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResolutionDropdown : MonoBehaviour
{
    private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;

    void Start()
    {
        resolutionDropdown = GetComponent<TMP_Dropdown>();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        foreach (Resolution res in resolutions)
        {
            if (res.refreshRateRatio.Equals(currentRefreshRate))
            {
                filteredResolutions.Add(res);
            }
        }

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            Resolution res = filteredResolutions[i];
            string optionText = res.width + " x " + res.height;
            options.Add(optionText);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height &&
                res.refreshRateRatio.Equals(Screen.currentResolution.refreshRateRatio))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRateRatio);
    }
}
