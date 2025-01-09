using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class VictoryScene : MonoBehaviour
    {
        [Header("UI Elements")]
        public TMP_Text narrativeText; // TextMeshPro for narrative
        public TMP_Text timerText; // TextMeshPro for timer display

        public GameObject restartButton; // Restart button
        public GameObject quitButton; // Quit button
        public float scrollSpeed = 50f; // Speed of the text scroll

        private string narrative = 
            "The Negotiator Prevented the War!\n\n\n\n\n"+ 
            "Through sheer determination and relentless effort, the Negotiator managed to deliver everything in time!\n" +
            "The villagers cheered as they admired the perfectly round potato and the lopsided wheelbarrow. The singing fish was placed in the village fountain, serenading everyone with its off-key melody.\n" +
            "Meanwhile, the pirates marveled at their newly fixed ship and the parrot with personality, who had already started cracking terrible jokes. Jerry the Skeleton, reunited with his pirate friends, danced joyfully in the sand.\n" +
            "With both sides satisfied, the tension dissolved, and instead of war, a grand feast was held at the beach, with villagers and pirates sharing stories, laughter, and perhaps too much rum.\n\n" +
            "\"Congratulations, Negotiator! You’ve not only saved the day but also created the most bizarre peace treaty in history. Are you ready to try again and beat your time?\"";
        
        private float timeTaken;

        void Start()
        {
             float completionTime = GameData.CompletionTime;

            int minutes = Mathf.FloorToInt(completionTime / 60);
            int seconds = Mathf.FloorToInt(completionTime % 60);
            int milliseconds = Mathf.FloorToInt((completionTime % 1) * 1000);
            // Hide buttons at first
            restartButton.SetActive(false);
            quitButton.SetActive(false);
            timerText.gameObject.SetActive(false);

            /*
            // Get time from GameStarterNPC
            GameStarterNPC gameStarterNPC = FindObjectOfType<GameStarterNPC>();
            float remainingTime = gameStarterNPC.GetRemainingTime();
            timerText.text = $"Time: {remainingTime}";
            */
            timerText.text = $"Completion Time: {minutes:00}:{seconds:00}:{milliseconds:000}";

            // Set the narrative text
            narrativeText.text = "";
            StartCoroutine(ScrollText());
        }

        // Coroutine to handle the scroll animation of the text
        IEnumerator ScrollText()
        {
            // Add the narrative to the text
            narrativeText.text = narrative;

            // Set the starting position for the text (off the screen at the bottom)
            RectTransform rectTransform = narrativeText.GetComponent<RectTransform>();
            float startPosY = -Screen.height / 2f;
            rectTransform.anchoredPosition = new Vector2(0, startPosY);

            // Scroll the text from bottom to top
            while (rectTransform.anchoredPosition.y < Screen.height / 2f + 500f)
            {
                rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
                yield return null;
            }

            // Once the text scroll is done, show buttons
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            timerText.gameObject.SetActive(true);
        }

        // Restart the scene
        public void RestartGame()
        {
            SceneManager.LoadScene("SampleScene"); // Replace "SampleScene" with your actual game scene name
        }

        // Quit the game
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif        
        }
    }
}
