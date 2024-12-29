using UnityEngine;
using UnityEngine.UI;

namespace Unity.FantasyKingdom
{
    public class ResQualFullSettings : MonoBehaviour
    {
        public Dropdown resolutionDropdown;
        public Dropdown qualityDropdown;
        public Toggle fullscreenToggle;

        void Start()
        {
            // Setup resolution dropdown
            resolutionDropdown.ClearOptions();
            foreach (Resolution res in Screen.resolutions)
            {
                resolutionDropdown.options.Add(new Dropdown.OptionData(res.ToString()));
            }

            // Set the current resolution
            resolutionDropdown.value = GetCurrentResolutionIndex();
            resolutionDropdown.RefreshShownValue();

            // Set quality dropdown
            qualityDropdown.ClearOptions();
            foreach (string quality in QualitySettings.names)
            {
                qualityDropdown.options.Add(new Dropdown.OptionData(quality));
            }

            // Set the current quality level
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            qualityDropdown.RefreshShownValue();

            // Set fullscreen toggle
            fullscreenToggle.isOn = Screen.fullScreen;

            // Load saved values
            LoadSettings();
        }

        public void ChangeResolution(int index)
        {
            Resolution newResolution = Screen.resolutions[index];
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
            PlayerPrefs.Save();
        }

        public void ChangeQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
            PlayerPrefs.SetInt("QualityIndex", index);
            PlayerPrefs.Save();
        }

        public void ToggleFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            // Load saved resolution
            if (PlayerPrefs.HasKey("ResolutionIndex"))
                resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");

            // Load saved quality setting
            if (PlayerPrefs.HasKey("QualityIndex"))
                qualityDropdown.value = PlayerPrefs.GetInt("QualityIndex");

            // Load saved fullscreen setting
            if (PlayerPrefs.HasKey("Fullscreen"))
                fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
        }

        private int GetCurrentResolutionIndex()
        {
            Resolution currentResolution = Screen.currentResolution;
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.resolutions[i].width == currentResolution.width &&
                    Screen.resolutions[i].height == currentResolution.height)
                {
                    return i;
                }
            }
            return 0; // Default to the first resolution
        }
    }
}
