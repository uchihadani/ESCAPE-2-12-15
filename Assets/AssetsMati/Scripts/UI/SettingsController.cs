using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Toggle toggleFullscreen;
    [SerializeField] private TMP_Dropdown dropdownResolution;
    [SerializeField] private Slider sliderMasterVolume;
    [SerializeField] private Slider sliderMouseSensitivity;

    [Header("Audio")]
    [SerializeField] private AudioMixer masterMixer;

    private Resolution[] _resolutions;
    private const string PP_FULLSCREEN = "Fullscreen";
    private const string PP_RESOLUTION_INDEX = "ResolutionIndex";
    private const string PP_MASTER_VOL_DB = "MasterVolDb";
    private const string PP_MOUSE_SENS = "MouseSensitivity";

    private void Awake()
    {
        _resolutions = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height, refreshRate = r.refreshRate })
            .GroupBy(r => (r.width, r.height))
            .Select(g => g.OrderByDescending(r => r.refreshRate).First())
            .OrderBy(r => r.width).ThenBy(r => r.height)
            .ToArray();

        dropdownResolution.ClearOptions();
        var options = _resolutions.Select(r => $"{r.width} x {r.height} @ {r.refreshRate}Hz").ToList();
        dropdownResolution.AddOptions(options);

      
        bool fullscreen = PlayerPrefs.GetInt(PP_FULLSCREEN, 1) == 1;
        int resIndex = Mathf.Clamp(PlayerPrefs.GetInt(PP_RESOLUTION_INDEX, _resolutions.Length - 1), 0, _resolutions.Length - 1);
        float volDb = PlayerPrefs.GetFloat(PP_MASTER_VOL_DB, 0f);          
        float mouseSens = PlayerPrefs.GetFloat(PP_MOUSE_SENS, 1.0f);        

        if (toggleFullscreen) toggleFullscreen.isOn = fullscreen;
        if (dropdownResolution) dropdownResolution.value = resIndex;
        if (sliderMasterVolume) sliderMasterVolume.value = Mathf.InverseLerp(-40f, 0f, volDb);
        if (sliderMouseSensitivity) sliderMouseSensitivity.value = mouseSens;

        ApplyResolution(resIndex, fullscreen);
        ApplyVolume(volDb);

        if (toggleFullscreen) toggleFullscreen.onValueChanged.AddListener(OnToggleFullscreen);
        if (dropdownResolution) dropdownResolution.onValueChanged.AddListener(OnResolutionChanged);
        if (sliderMasterVolume) sliderMasterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (sliderMouseSensitivity) sliderMouseSensitivity.onValueChanged.AddListener(OnMouseSensitivityChanged);
    }

    private void OnDestroy()
    {
        if (toggleFullscreen) toggleFullscreen.onValueChanged.RemoveListener(OnToggleFullscreen);
        if (dropdownResolution) dropdownResolution.onValueChanged.RemoveListener(OnResolutionChanged);
        if (sliderMasterVolume) sliderMasterVolume.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        if (sliderMouseSensitivity) sliderMouseSensitivity.onValueChanged.RemoveListener(OnMouseSensitivityChanged);
    }

    private void OnToggleFullscreen(bool isFullscreen)
    {
        ApplyResolution(dropdownResolution.value, isFullscreen);
        PlayerPrefs.SetInt(PP_FULLSCREEN, isFullscreen ? 1 : 0);
    }

    private void OnResolutionChanged(int index)
    {
        bool isFullscreen = toggleFullscreen ? toggleFullscreen.isOn : Screen.fullScreen;
        ApplyResolution(index, isFullscreen);
        PlayerPrefs.SetInt(PP_RESOLUTION_INDEX, index);
    }

    private void OnMasterVolumeChanged(float slider01)
    {
        float volDb = Mathf.Lerp(-40f, 0f, slider01);
        ApplyVolume(volDb);
        PlayerPrefs.SetFloat(PP_MASTER_VOL_DB, volDb);
    }

    private void OnMouseSensitivityChanged(float sens)
    {
        PlayerPrefs.SetFloat(PP_MOUSE_SENS, sens);
    }

    private void ApplyResolution(int index, bool fullscreen)
    {
        if (_resolutions.Length == 0) return;
        index = Mathf.Clamp(index, 0, _resolutions.Length - 1);
        var r = _resolutions[index];
        Screen.SetResolution(r.width, r.height, fullscreen, r.refreshRate);
    }

    private void ApplyVolume(float volDb)
    {
        if (masterMixer) masterMixer.SetFloat("MasterVolume", volDb);
        AudioListener.volume = masterMixer ? 1f : Mathf.InverseLerp(-40f, 0f, volDb);
    }
}
