using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class LopsidedWheelbarrowInteraction : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI;    // "Press E to Pick Up Wheelbarrow" message
        [SerializeField] private TMP_Text taskListText; // Reference to the task list UI text
        [SerializeField] private string taskDescription = "A Lopsided Wheelbarrow"; // Task description

        private bool playerInRange = false; // Whether the player is near the wheelbarrow
        private bool taskCompleted = false; // Whether the task has been completed

        public GameStarterNPC gameStarterNPC; // Reference to the GameStarterNPC script

        public GameObject taskPointer;



        private void Start()
        {
            interactionUI.SetActive(false); // Hide interaction UI initially
        }

        private void Update()
        {
            if (taskCompleted) return;

            // Check for interaction input
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                PickUpWheelbarrow();
            }
        }

        private void PickUpWheelbarrow()
        {
            taskCompleted = true;

            // Update the task in the UI (turn it green)
            string completedTask = $"<color=green>{taskDescription}</color>";
            taskListText.text = taskListText.text.Replace(taskDescription, completedTask);
            taskPointer.SetActive(false);

            gameStarterNPC.TaskCompleted(4); // Assuming this is the first task in the list


            // Hide the wheelbarrow and interaction UI
            gameObject.SetActive(false);
            interactionUI.SetActive(false);

            Debug.Log("Lopsided wheelbarrow picked up! Task complete.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !taskCompleted)
            {
                playerInRange = true;

                // Show the interaction UI
                interactionUI.SetActive(true);
                interactionUI.GetComponent<TMP_Text>().text = "Press E to Pick Up Wheelbarrow";
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionUI.SetActive(false); // Hide the interaction message
            }
        }
    }
}
