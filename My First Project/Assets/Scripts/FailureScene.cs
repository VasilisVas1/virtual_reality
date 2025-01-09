using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class FailureScene : MonoBehaviour
    {
        [Header("UI Elements")]
        public TMP_Text narrativeText; // TextMeshPro text for narrative
        public GameObject restartButton; // Restart button
        public GameObject quitButton; // Quit button
        public float scrollSpeed = 50f; // Speed of the text scroll

        private string narrative =
            "The Negotiator Couldn't Stop the War!\n\n\n\n\n"+ 
            "The clock struck zero, and chaos erupted!\n" +
            "The villagers, armed with pitchforks and an unusual amount of perfectly round potatoes, marched toward the pirates' camp.\n" +
            "Meanwhile, the pirates, dragging their freshly fixed ship and their squawking parrot, charged toward the village.\n" +
            "Amid the turmoil, Jerry the Skeleton cheered them on from the sidelines, yelling, \"Don’t forget the singing fish!\"\n" +
            "Unfortunately, the Negotiator's efforts fell short, and the war began with an epic splash of absurdity and chaos.\n" +
            "The villagers and pirates clashed, and the world will forever remember the Great Battle of the Singing Fish and Perfectly Round Potatoes.\n\n"+
            "\"Better luck next time, Negotiator. Can you be faster and smarter in the next run?\"";


        void Start()
        {
            // Hide buttons at first
            restartButton.SetActive(false);
            quitButton.SetActive(false);

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
            while (rectTransform.anchoredPosition.y < Screen.height / 2f+400f)
            {
                rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
                yield return null;
            }

            // Once the text scroll is done, show buttons
            restartButton.SetActive(true);
            quitButton.SetActive(true);
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
