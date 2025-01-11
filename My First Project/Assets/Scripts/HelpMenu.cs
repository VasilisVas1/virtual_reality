using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public GameObject helpMenuUI; // Reference to the Help Menu UI
    public GameObject taskListText; // Reference to the task list text
    public GameObject timerText; // Reference to the timer text
    public Camera playerCamera; // Reference to the player's camera
    public Camera boatCamera; // Reference to the boat's camera
    public GameObject player; // Reference to the player GameObject
    public MonoBehaviour playerController; // Reference to the player controller

    private bool isHelpMenuActive = false;
    private bool wasOnBoat = false; // Track if the player was on the boat when opening the help menu

    public PauseMenu pauseMenu; // Reference to the PauseMenu script


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (pauseMenu != null && pauseMenu.IsPauseMenuActive()) return;

            if (isHelpMenuActive)
            {
                CloseHelpMenu();
            }
            else
            {
                OpenHelpMenu();
            }
        }
    }
    public bool IsHelpMenuActive()
    {
        return isHelpMenuActive;
    }


    public void OpenHelpMenu()
    {
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

        if (taskListText.gameObject.activeSelf)
        {
            taskListText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
        }

        helpMenuUI.SetActive(true);
        Time.timeScale = 0f;
        DisablePlayerController();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isHelpMenuActive = true;
    }

    public void CloseHelpMenu()
    {
        if (!taskListText.gameObject.activeSelf)
        {
            taskListText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
        }

        helpMenuUI.SetActive(false);
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
        isHelpMenuActive = false;
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
}
