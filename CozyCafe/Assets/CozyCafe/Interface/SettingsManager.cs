using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    // Fields
    [SerializeField] private AudioMixer mixer;

    private Resolution[] availableResolutions;

    private const float MIN_VOLUME = -80f;
    private const float MAX_VOLUME = 0f;

    private const float MIN_VOLUME_SLIDER_VALUE = 0f;
    private const float MAX_VOLUME_SLIDER_VALUE = 100f;

    public static readonly FullScreenMode[] SUPPORTED_WINDOW_MODES = new FullScreenMode[]
    {
            FullScreenMode.ExclusiveFullScreen,
            FullScreenMode.FullScreenWindow,
            FullScreenMode.Windowed
    };

    public static readonly string[] WINDOW_MODE_NAMES = new string[]
    {
            "Full screen",
            "Borderless window",
            "Window",
    };

    private const string FILE_NAME = "settings.json";

    private Settings settings;

    // Properties
    public static SettingsManager Instance { get; private set; }
    public static Settings Settings => Instance != null ? Instance.settings : null;
    public Resolution[] AvailableResolutions { get { return availableResolutions; } }
    private string SettingsPath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    private Resolution preSaveRes;
    private FullScreenMode preSaveMode;

    // Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        availableResolutions = Screen.resolutions;
        LoadSettings();
        ApplySettings();
    }

    private void Start()
    {
    }

    public void LoadSettings()
    {
        if (File.Exists(SettingsPath))
        {
            string json = File.ReadAllText(SettingsPath);
            settings = JsonUtility.FromJson<Settings>(json);
        }
        else
        {
            settings = Settings.CreateDefault();
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        if (settings == null)
        {
            Debug.LogError($"Cant save NULL");
            return;
        }
        //try catch error
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(SettingsPath, json);
    }

    public void ApplySettings()
    {
        ApplyVideoSettings();
        ApplyAudioSettings();
    }

    public void SetNewResolution(int newValue)
    {
        Debug.Log($"Set New Resolution to {availableResolutions[newValue].width} x {availableResolutions[newValue].height}");
        preSaveRes.width = availableResolutions[newValue].width;
        preSaveRes.height = availableResolutions[newValue].height;
    }

    internal void SetScreenSetting(int newValue)
    {
        preSaveMode = SUPPORTED_WINDOW_MODES[newValue];
    }

    internal void SetMasterVolume(float newValue)
    {
        settings.masterVolume = (int)newValue;
        ApplyAudioSettings();
    }

    internal void SetMusicVolume(float newValue)
    {
        settings.musicVolume = (int)newValue;
        ApplyAudioSettings();
    }

    internal void SetEffectVolume(float newValue)
    {
        settings.sfxVolume = (int)newValue;
        ApplyAudioSettings();
    }

    internal void SetBabbleBool(bool newValue)
    {
        settings.babble = newValue;
        ApplyAudioSettings();
    }

    public void ApplyVideoSettings()
    {
        if (preSaveRes.width > 0 && preSaveRes.height > 0f)
        {
            settings.resolutionX = preSaveRes.width;
            settings.resolutionY = preSaveRes.height;
        }

        settings.fullScreenMode = preSaveMode;
        //Debug.Log($"Set Screen Setting to {preSaveMode}");

        Screen.SetResolution(settings.resolutionX, settings.resolutionY, settings.fullScreenMode, settings.RefreshRate);
    }


    // --- Protected/Private Methods ----------------------------------------------------------------------------------        

    private void ApplyAudioSettings()
    {
        //SetVolume("MasterVolume", settings.masterVolume);
        //SetVolume("MusicVolume", settings.musicVolume);
        //SetVolume("SfxVolume", settings.sfxVolume);
        //SetVolume("Babble", settings.babble ? settings.sfxVolume : 0);
    }

    private void SetVolume(string parameter, float value)
    {
        float t = Mathf.InverseLerp(MIN_VOLUME_SLIDER_VALUE, MAX_VOLUME_SLIDER_VALUE, value);
        float newVolume = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, t);
        mixer.SetFloat(parameter, newVolume);
    }


}
