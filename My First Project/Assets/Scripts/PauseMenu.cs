using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI
    public MonoBehaviour playerController; // Reference to the script controlling player movement and camera
    public GameObject defaultButton; // Button to select when the menu opens (optional)

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        pauseMenuUI.SetActive(true); // Display the pause menu
        Time.timeScale = 0f; // Freeze game time
        DisablePlayerController(); // Disable player controls
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        ResetButtonSelection(); // Reset button selection
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume game time
        EnablePlayerController(); // Re-enable player controls
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the game view
        Cursor.visible = false; // Hide the cursor
        isPaused = false;
    }

    private void DisablePlayerController()
    {
        if (playerController != null)
        {
            playerController.enabled = false; // Disable the player controller script
        }
    }

    private void EnablePlayerController()
    {
        if (playerController != null)
        {
            playerController.enabled = true; // Enable the player controller script
        }
    }

    private void ResetButtonSelection()
    {
        // Deselect any currently selected button
        EventSystem.current.SetSelectedGameObject(null);

        // Optionally, set a default button to be selected
        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // Debug message for testing in the editor
        Application.Quit(); // Quit the application
    }
}
