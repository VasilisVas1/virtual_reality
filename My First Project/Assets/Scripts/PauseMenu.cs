using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public MonoBehaviour playerController;
    public GameObject settingsPanel;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public AudioSource musicAudioSource;

    public GameObject taskListText;    // Display tasks to the player
    public GameObject timerText;       

    public Camera playerCamera; // Reference to the player's camera
    public Camera boatCamera;   // Reference to the boat's camera
    public GameObject player;   // Reference to the player GameObject

    public HelpMenu helpMenu; // Reference to the HelpMenu script

    private bool isPaused = false;
    private bool isInSettings = false;
    private bool wasOnBoat = false; // Track if the player was on the boat when pausing

    void Start()
    {
        // Ensure default volumes are set
        masterVolumeSlider.value = AudioListener.volume;

        //musicAudioSource.volume = 0.5f;
        musicVolumeSlider.value = musicAudioSource.volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            // Prevent opening Pause Menu if Help Menu is active
            if (helpMenu != null && helpMenu.IsHelpMenuActive()) return;

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

        // Detect which camera is active and switch to the player's camera
        if (boatCamera.enabled)
        {
            wasOnBoat = true;
            boatCamera.enabled = false;
            playerCamera.enabled = true;

            // Reactivate the player GameObject for rendering the menu
            if (!player.activeSelf)
            {
                player.SetActive(true);
            }
        }

        if(taskListText.gameObject.activeSelf){
            taskListText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        DisablePlayerController();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public bool IsPauseMenuActive()
    {
        return isPaused;
    }


    public void Resume()
    {
        if (isInSettings) return; // Don't allow Resume if in Settings

        if(!taskListText.gameObject.activeSelf){
            taskListText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        EnablePlayerController();

        // Restore the previous camera state
        if (wasOnBoat)
        {
            playerCamera.enabled = false;
            boatCamera.enabled = true;

            // Hide the player GameObject again after resuming
            player.SetActive(false);
            wasOnBoat = false;
        }

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
