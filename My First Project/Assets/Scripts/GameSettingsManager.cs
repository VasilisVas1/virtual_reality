using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class GameSettingsManager : MonoBehaviour
    {
        void Start()
    {
        // Apply saved master volume
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            AudioListener.volume = savedVolume;
        }

        // Apply saved resolution
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            Resolution savedResolution = Screen.resolutions[savedResolutionIndex];
            Screen.SetResolution(savedResolution.width, savedResolution.height, Screen.fullScreen);
        }

        // Apply saved quality setting
        if (PlayerPrefs.HasKey("QualityIndex"))
        {
            int savedQualityIndex = PlayerPrefs.GetInt("QualityIndex");
            QualitySettings.SetQualityLevel(savedQualityIndex);
        }

        // Apply saved fullscreen setting
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            Screen.fullScreen = isFullscreen;
        }
    }
    }
}
