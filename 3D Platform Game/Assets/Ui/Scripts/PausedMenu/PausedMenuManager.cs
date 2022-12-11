using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PausedMenuManager : MonoBehaviour
{
    [Header("Container Menu")]
    [SerializeField] GameObject pausedMenuContainer;
    [SerializeField] GameObject OptionMenuContainer;

    [Header("Dialog Menu")]
    [SerializeField] GameObject returnToMenuDialog;
    [SerializeField] GameObject soundMenuDialog;
    [SerializeField] GameObject graphicsMenuDialog;


    [Header("Load Scene")]
    [SerializeField] string sceneToLoad;

    [Header("Volume Settings")]
    [SerializeField] private TextMeshProUGUI volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 1;

    [Header("Graphics settings")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TextMeshProUGUI brightnessTextValue;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private bool _isInAnotherMenu;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private void Update()
    {
        if (!GameLogic.isGameOver && !GameLogic.isGameStart)
        {
            if (UIInputHandler.isPaused && !_isInAnotherMenu)
            {
                ActivatePausedMenu();
            }
            else if (!UIInputHandler.isPaused)
            {
                DeactivatePausedMenu();

            }
        }
    }

    public void ActivatePausedMenu()
    {
        UIInputHandler.isPaused = true;
        Time.timeScale = 0;
        pausedMenuContainer.SetActive(true);
    }

    public void DeactivatePausedMenu()
    {
        Time.timeScale = 1;

        pausedMenuContainer.SetActive(false);
        OptionMenuContainer.SetActive(false);

        returnToMenuDialog.SetActive(false);
        soundMenuDialog.SetActive(false);
        graphicsMenuDialog.SetActive(false);

        UIInputHandler.isPaused = false;
        SetIsInAnotherMenuFalse();
    }

    public void ReturnToMenuDialogYes()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SetIsInAnotherMenuTrue()
    {
        _isInAnotherMenu = true;
    }

    public void SetIsInAnotherMenuFalse()
    {
        _isInAnotherMenu = false;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("MasterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("MasterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("MasterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

    }

    public void ResetButton(string menuType)
    {
        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (menuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;
        }

        Resolution currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
        resolutionDropdown.value = currentResolution.height;
        GraphicsApply();
    }
}
