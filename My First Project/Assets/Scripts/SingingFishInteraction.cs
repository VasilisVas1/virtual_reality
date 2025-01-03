using System.Collections;
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class SingingFishInteraction : MonoBehaviour
    {
        [Header("Fish Settings")]
        public AudioSource fishAudio;       // The audio source for the fish
        public float maxVolume = 1f;        // Maximum volume of the singing
        public float maxDistance = 10f;     // Maximum distance for the audio to be heard at full volume
        private bool isSingingFish = false; // Whether this fish is the singing fish

        [Header("UI Elements")]
        public GameObject interactionUI;    // "Press E to Pick Up Fish" message
        private TMP_Text taskListText;      // Reference to the task list UI text
        private string taskDescription;     // Task description

        private bool playerInRange = false; // Whether the player is near the fish
        private bool taskCompleted = false; // Whether the task has been completed
        private Transform playerTransform;  // Reference to the player transform

        private void Start()
        {
            fishAudio.volume = 0f; // Start with the audio muted
            interactionUI.SetActive(false); // Hide interaction UI initially
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (taskCompleted) return;

            // Adjust volume based on distance
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            fishAudio.volume = Mathf.Clamp01(1 - (distance / maxDistance)) * maxVolume;

            // Check for interaction input
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && isSingingFish)
            {
                PickUpFish();
            }
        }

        public void SetAsSingingFish(TMP_Text taskList, string taskDesc)
        {
            isSingingFish = true;
            taskListText = taskList;
            taskDescription = taskDesc;
        }

        private void PickUpFish()
        {
            taskCompleted = true;

            // Update the task in the UI (turn it green)
            string completedTask = $"<color=green>{taskDescription}</color>";
            taskListText.text = taskListText.text.Replace(taskDescription, completedTask);

            // Hide the fish and interaction UI
            gameObject.SetActive(false);
            interactionUI.SetActive(false);

            // Stop the fish audio
            fishAudio.Stop();

            Debug.Log("Singing fish picked up! Task complete.");
        }

        private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player") && !taskCompleted)
    {
        playerInRange = true;

        // Show the interaction UI only if this is the singing fish
        if (isSingingFish)
        {
            interactionUI.SetActive(true);
            interactionUI.GetComponent<TMP_Text>().text = "Press E to Pick Up Fish";
        }
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
