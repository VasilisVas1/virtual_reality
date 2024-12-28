using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.FantasyKingdom
{
    public class MainMenu : MonoBehaviour
    {
        // References to the main menu panel and settings panel
        public GameObject mainMenuPanel;
        public GameObject settingsPanel;

        // Called when the player clicks "Start Game"
        public void StartGame()
        {
            SceneManager.LoadScene("SampleScene"); // Replace "SampleScene" with your actual game scene name
        }

        // Called when the player clicks "Settings"
        public void OpenSettings()
        {
            // Hide the main menu and show the settings panel
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        // Called when the player clicks "Quit Game"
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
