using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject mainMenuPanel;
        public GameObject settingsPanel;

        // Variable to store the current mode ("Virtual Reality" or "Game Mode")
        private string currentMode = "Game Mode";

        // Reference to the mode selection button's text
        public TMP_Text modeButtonText;

        private void Start()
        {
            // Set the default mode if needed
            currentMode = "Game Mode";
            UpdateModeButtonText();

            
            Time.timeScale = 1f;

        }

        public void ToggleMode()
        {
            // Toggle between "Virtual Reality" and "Game Mode"
            if (currentMode == "Game Mode")
                currentMode = "Virtual Reality";
            else
                currentMode = "Game Mode";

            UpdateModeButtonText();
        }

        private void UpdateModeButtonText()
        {
            // Update the button text based on the current mode
            modeButtonText.text = currentMode;
        }

        public void StartGame()
        {
            // Store the selected mode so it can be accessed in the next scene
            PlayerPrefs.SetString("SelectedMode", currentMode);
            PlayerPrefs.Save();

            SceneManager.LoadScene("SampleScene");
        }

        public void OpenSettings()
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting the game...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
