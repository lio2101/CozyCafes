using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string exposedVolumeParameter;

    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = 1f;
        SetVolume(1f);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        //loading
        //if (PlayerPrefs.HasKey(exposedVolumeParameter))
        //{
        //    float savedVolume = PlayerPrefs.GetFloat(exposedVolumeParameter);
        //    volumeSlider.value = Mathf.Pow(10, savedVolume / 20); // Convert dB to linear slider value
        //    SetVolume(volumeSlider.value);
        //}
    }

    public void SetVolume(float sliderValue)
    {
        float volumeInDB = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat(exposedVolumeParameter, volumeInDB);

        //PlayerPrefs.SetFloat(exposedVolumeParameter, volumeInDB);
    }
}
