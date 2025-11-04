using UnityEngine;

[System.Serializable]
public class Settings
{
    // Video settings
    public FullScreenMode fullScreenMode;
    public int resolutionX;
    public int resolutionY;
    public uint refreshRateNumerator;
    public uint refreshRateDenominator;

    // Audio settings
    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;

    public Resolution Resolution => new()
    {
        width = resolutionX,
        height = resolutionY,
        refreshRateRatio = RefreshRate
    };

    public RefreshRate RefreshRate => new()
    {
        numerator = refreshRateNumerator,
        denominator = refreshRateDenominator,
    };

    public Settings()
    {
    }

    public static Settings CreateDefault()
    {
        Settings s = new();

        s.fullScreenMode = Screen.fullScreenMode;
        s.SetResolution(Screen.currentResolution);

        s.masterVolume = 70;
        s.musicVolume = 70;
        s.sfxVolume = 70;

        return s;
    }

    public void SetResolution(Resolution r)
    {
        resolutionX = r.width;
        resolutionY = r.height;
        refreshRateNumerator = r.refreshRateRatio.numerator;
        refreshRateDenominator = r.refreshRateRatio.denominator;
    }
}
