using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public MonoBehaviour playerController; 
    public GameObject settingsPanel; 
    public Slider masterVolumeSlider; 
    public Slider musicVolumeSlider; 
    public AudioSource musicAudioSource; 

    private bool isPaused = false;
    private bool isInSettings = false; // Track if the settings panel is open

    void Start()
    {
        // Ensure default volumes are set
        AudioListener.volume = 1f; 
        masterVolumeSlider.value = AudioListener.volume;

        musicAudioSource.volume = 0.5f; 
        musicVolumeSlider.value = musicAudioSource.volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInSettings)
            {
                CloseSettings(); // Close settings if open
            }
            else if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (isInSettings) return; // Don't allow Pause if in Settings

        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        DisablePlayerController(); 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
        isPaused = true;
    }

    public void Resume()
    {
        if (isInSettings) return; // Don't allow Resume if in Settings

        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; 
        EnablePlayerController(); 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        isPaused = false;
    }

    private void DisablePlayerController()
    {
        if (playerController != null)
        {
            playerController.enabled = false; 
        }
    }

    private void EnablePlayerController()
    {
        if (playerController != null)
        {
            playerController.enabled = true; 
        }
    }

    public void ShowSettings()
    {
        pauseMenuUI.SetActive(false); // Deactivate Pause Menu
        settingsPanel.SetActive(true); 
        isInSettings = true; 
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); 
        pauseMenuUI.SetActive(true); // Reactivate Pause Menu
        isInSettings = false; 
    }

    public void UpdateMasterVolume(float volume)
    {
        AudioListener.volume = volume; 
    }

    public void UpdateMusicVolume(float volume)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = volume; 
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
