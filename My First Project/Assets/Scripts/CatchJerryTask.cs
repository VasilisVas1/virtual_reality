using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class CatchJerryTask : MonoBehaviour
    {
        public TMP_Text taskListText;        // Reference to the task list UI text
        public string taskDescription = "Catch Jerry the Goblin"; // Task description
        public GameObject interactionUI;    // UI element to display the interaction message
        public GameObject jerryGameObject;  // Reference to Jerry's GameObject

        private bool playerInRange = false; // Tracks if the player is in range of Jerry
        private bool taskCompleted = false; // Tracks if the task is completed

        private void Update()
        {
            // Check if the player presses E while in range and the task is not completed
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !taskCompleted)
            {
                CatchJerry();
            }
        }

        private void CatchJerry()
        {
            taskCompleted = true;

            // Update the task in the UI
            if (taskListText != null)
            {
                string completedTask = $"<color=green>{taskDescription}</color>";
                taskListText.text = taskListText.text.Replace(taskDescription, completedTask);
            }

            // Hide the interaction UI
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }

            // Make Jerry disappear
            if (jerryGameObject != null)
            {
                jerryGameObject.SetActive(false);
            }

            // Perform any additional actions for catching Jerry (e.g., animations, sound effects, etc.)
            Debug.Log("Jerry has been caught!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !taskCompleted)
            {
                playerInRange = true;

                // Display interaction UI
                if (interactionUI != null)
                {
                    interactionUI.SetActive(true);
                    interactionUI.GetComponent<TMP_Text>().text = "Press E to Catch Jerry";
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;

                // Hide interaction UI
                if (interactionUI != null)
                {
                    interactionUI.SetActive(false);
                }
            }
        }
    }
}
