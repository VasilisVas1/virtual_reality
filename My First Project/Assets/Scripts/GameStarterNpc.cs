using System.Collections;
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class GameStarterNPC : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI; // "Press E to Start Tasks" message
        [SerializeField] private TMP_Text interactionText; // Text for "Press E to start tasks"
        [SerializeField] private TMP_Text taskListText;    // Display tasks to the player
        [SerializeField] private TMP_Text timerText;       // Timer display

        [Header("NPC Info")]
        public string npcName;          // Name of the NPC
        public string[] tasks;          // List of tasks to assign to the player

        private bool playerInRange = false;  // Is the player in range of the NPC
        private bool gameStarted = false;    // Has the game already started

        [Header("Game Settings")]
        public float taskTimerDuration = 300f; // Task timer duration in seconds
        private float taskTimer;

        public GameObject potato;            // The potato object to be activated
        public GameObject[] parrots;         // Array of parrot GameObjects to be activated

        [Header("Fish Settings")]
        public GameObject[] fishes;         // Array of fish GameObjects to be activated

        [Header("Wheelbarrow Settings")]
        public GameObject[] lopsidedWheelbarrows; // Array of lopsided wheelbarrow GameObjects to be activated

        private Coroutine taskTimerCoroutine; // Reference for the timer coroutine

        void Start()
        {
            interactionUI.SetActive(false);  // Hide "Press E to Start Tasks" initially
            taskListText.gameObject.SetActive(false); // Hide tasks list initially
            timerText.gameObject.SetActive(false);    // Hide timer initially

            // Deactivate potato and parrots at the start
            potato.SetActive(false);
            foreach (GameObject parrot in parrots)
            {
                parrot.SetActive(false);
            }

            // Deactivate all fishes at the start
            foreach (GameObject fish in fishes)
            {
                fish.SetActive(false);
            }

            // Deactivate all lopsided wheelbarrows at the start
            foreach (GameObject wheelbarrow in lopsidedWheelbarrows)
            {
                wheelbarrow.SetActive(false);
            }
        }

        void Update()
        {
            // Check for interaction input
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !gameStarted)
            {
                StartGame();
            }
        }

        void StartGame()
        {
            gameStarted = true;

            // Activate the potato and parrots
            potato.SetActive(true);
            foreach (GameObject parrot in parrots)
            {
                parrot.SetActive(true);
            }

            foreach (GameObject fish in fishes)
{
    fish.SetActive(true); // Reactivate the fish
    var audioSource = fish.GetComponent<AudioSource>();
    if (audioSource != null)
    {
        audioSource.Stop(); // Ensure a fresh start
        audioSource.Play(); // Start playback again
    }
}


            // Activate all lopsided wheelbarrows
            foreach (GameObject wheelbarrow in lopsidedWheelbarrows)
            {
                wheelbarrow.SetActive(true);
            }

            // Hide interaction UI
            interactionUI.SetActive(false);

            // Display task list
            taskListText.gameObject.SetActive(true);
            taskListText.text = "Tasks:\n";
            foreach (string task in tasks)
            {
                taskListText.text += $"- {task}\n";
            }

            // Show and initialize the timer
            timerText.gameObject.SetActive(true);
            taskTimer = taskTimerDuration;
            taskTimerCoroutine = StartCoroutine(TaskTimer());
        }

        IEnumerator TaskTimer()
        {
            while (taskTimer > 0)
            {
                taskTimer -= Time.deltaTime;

                // Update timer text
                UpdateTimerDisplay();

                yield return null;
            }

            EndGame();
        }

        void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(taskTimer / 60);
            int seconds = Mathf.FloorToInt(taskTimer % 60);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        void EndGame()
        {
            Debug.Log("Time's up! Tasks are finished.");

            // Hide the timer
            timerText.gameObject.SetActive(false);

            // Add logic here for ending the game or showing results
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the collider is the player
            {
                playerInRange = true;
                if (!gameStarted)
                {
                    interactionUI.SetActive(true); // Show "Press E to Start Tasks"
                    interactionText.text = $"Press E to Start Tasks with {npcName}";
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionUI.SetActive(false); // Hide "Press E to Start Tasks"
            }
        }
    }
}
