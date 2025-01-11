using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For loading scenes

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
        private bool[] taskCompleted;        // Task completion status

        [Header("Game Settings")]
        public float taskTimerDuration = 300f; // Task timer duration in seconds
        private float taskTimer;

        public GameObject potato;            // The potato object to be activated
        public GameObject invisibleWall;            // The potato object to be activated
        public GameObject invisibleWall2;            // The potato object to be activated
        public GameObject[] parrots;         // Array of parrot GameObjects to be activated
        public GameObject[] fishes;         // Array of fish GameObjects to be activated
        public GameObject[] lopsidedWheelbarrows; // Array of lopsided wheelbarrow GameObjects to be activated

        public GameObject[] accessBarriers; 

        public GameObject compass;

        private Coroutine taskTimerCoroutine; // Reference for the timer coroutine

        [Header("Music Settings")]
        public AudioSource backgroundAudioSource; // The AudioSource component
        public AudioClip newBackgroundMusic;      // The new music to play

        void Start()
        {
            interactionUI.SetActive(false);  // Hide "Press E to Start Tasks" initially
            taskListText.gameObject.SetActive(false); // Hide tasks list initially
            timerText.gameObject.SetActive(false);    // Hide timer initially

            // Deactivate objects at the start
            potato.SetActive(false);
            invisibleWall.SetActive(true);
            invisibleWall2.SetActive(true);
            foreach (GameObject parrot in parrots) parrot.SetActive(false);
            foreach (GameObject fish in fishes) fish.SetActive(false);
            foreach (GameObject wheelbarrow in lopsidedWheelbarrows) wheelbarrow.SetActive(false);


            // Initialize task completion array
            taskCompleted = new bool[tasks.Length];
        }

        void Update()
        {
            // Check for interaction input
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !gameStarted)
            {
                StartGame();
            }

            // Check if all tasks are completed
            if (gameStarted && AreAllTasksCompleted())
            {
                WinGame();
            }
        }

        void StartGame()
        {
            gameStarted = true;

            // Activate game objects
            potato.SetActive(true);
            compass.SetActive(true);

            invisibleWall.SetActive(false);
            invisibleWall2.SetActive(false);
            foreach (GameObject parrot in parrots) parrot.SetActive(true);
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


            foreach (GameObject wheelbarrow in lopsidedWheelbarrows) wheelbarrow.SetActive(true);
            foreach (GameObject accessBarrier in accessBarriers) accessBarrier.SetActive(false);


            // Play background music
            if (backgroundAudioSource != null && newBackgroundMusic != null)
            {
                backgroundAudioSource.clip = newBackgroundMusic;
                backgroundAudioSource.Play();
            }

            // Hide interaction UI
            interactionUI.SetActive(false);

            // Display task list
            taskListText.gameObject.SetActive(true);
            taskListText.text = "Tasks:(Press T to close)\n";
            for (int i = 0; i < tasks.Length; i++)
            {
                taskListText.text += $"- {tasks[i]}\n";
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
            int milliseconds = Mathf.FloorToInt((taskTimer % 1) * 1000);

            timerText.text = $"Time Left: {minutes:00}:{seconds:00}:{milliseconds:000}";
        }

    
        void EndGame()
        {
            Debug.Log("Time's up! Tasks are finished.");
            if (AreAllTasksCompleted())
            {
                WinGame();
            }
            else
            {
                LoseGame();
            }
        }

        void WinGame()
        {
            // Calculate completion time
            float completionTime = taskTimerDuration - taskTimer;
            GameData.CompletionTime = completionTime;
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Load VictoryScene
            Debug.Log("You won!");
            SceneManager.LoadScene("VictoryScene");

            // Show victory message (optional)
            Debug.Log("Victory! All tasks completed on time.");
        }

        void LoseGame()
        {
            // Load FailureScene
            Debug.Log("You lost!");
            SceneManager.LoadScene("FailureScene");

            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Show failure message (optional)
            Debug.Log("Failure! Time's up and not all tasks are completed.");
        }

        bool AreAllTasksCompleted()
        {
            foreach (bool task in taskCompleted)
            {
                if (!task) return false; // If any task is not complete, return false
            }
            return true; // All tasks are completed
        }

        public void TaskCompleted(int taskIndex)
        {
            if (taskIndex >= 0 && taskIndex < tasks.Length)
            {
                taskCompleted[taskIndex] = true;
                // You can also change the task color to green here if needed
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                if (!gameStarted)
                {
                    interactionUI.SetActive(true);
                    interactionText.text = $"Press E to Start Game {npcName}";
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionUI.SetActive(false);
            }
        }
    }
}