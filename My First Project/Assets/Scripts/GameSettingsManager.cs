using UnityEngine; 

namespace Unity.FantasyKingdom
{
    public class GameSettingsManager : MonoBehaviour
    {
        public AudioSource musicAudioSource; // Αναφορά στην πηγή ήχου για τη μουσική

        void Start()
        {
            // Εφαρμογή της αποθηκευμένης έντασης του ήχου
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
                AudioListener.volume = savedVolume;
            }

            // Εφαρμογή της αποθηκευμένης έντασης της μουσικής
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
                if (musicAudioSource != null)
                {
                    musicAudioSource.volume = savedMusicVolume;
                }
            }

            // Εφαρμογή της αποθηκευμένης ανάλυσης οθόνης
            if (PlayerPrefs.HasKey("ResolutionIndex"))
            {
                int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
                Resolution savedResolution = Screen.resolutions[savedResolutionIndex];
                Screen.SetResolution(savedResolution.width, savedResolution.height, Screen.fullScreen);
            }

            // Εφαρμογή της αποθηκευμένης ρύθμισης ποιότητας γραφικών
            if (PlayerPrefs.HasKey("QualityIndex"))
            {
                int savedQualityIndex = PlayerPrefs.GetInt("QualityIndex");
                QualitySettings.SetQualityLevel(savedQualityIndex);
            }

            // Εφαρμογή της αποθηκευμένης ρύθμισης πλήρους οθόνης
            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
                Screen.fullScreen = isFullscreen;
            }
        }
    }
}
