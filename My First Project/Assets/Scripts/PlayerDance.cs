using System.Collections;
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class PlayerDance : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI;     // "Press E to Dance" message
        [SerializeField] private TMP_Text interactionText; // Text for "Press E to Dance"

        [Header("Dance Settings")]
        public float danceSpeed = 10f;       // Speed of the up-and-down motion (higher = faster)
        public float danceHeight = 0.5f;     // Height of the up-and-down motion

        private bool isDancing = false;      // Is the player currently dancing
        private bool inDanceZone = false;    // Is the player in a dance zone
        private Vector3 originalPosition;    // Player's original position before dancing

        [Header("Player References")]
        public MonoBehaviour playerMovementScript; // Reference to player's movement script

        private Coroutine danceCoroutine;    // Reference to the dance coroutine

        void Start()
        {
            interactionUI.SetActive(false); // Hide "Press E to Dance" initially
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && inDanceZone)
            {
                if (!isDancing)
                {
                    StartDancing();
                }
                else
                {
                    StopDancing();
                }
            }
        }

        void StartDancing()
        {
            isDancing = true;
            interactionText.text = "Press E to Stop Dancing"; // Update the message

            // Disable player movement
            if (playerMovementScript != null) playerMovementScript.enabled = false;

            // Save the original position
            originalPosition = transform.position;

            // Start dancing
            danceCoroutine = StartCoroutine(Dance());
        }

        IEnumerator Dance()
        {
            while (isDancing)
            {
                // Calculate the up-and-down movement relative to the original position
                float newY = originalPosition.y + Mathf.Sin(Time.time * danceSpeed) * danceHeight;
                transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
                yield return null;
            }
        }

        void StopDancing()
        {
            isDancing = false;

            // Reset player position to the original position
            transform.position = originalPosition;

            // Enable player movement
            if (playerMovementScript != null) playerMovementScript.enabled = true;

            // Stop the dance coroutine
            if (danceCoroutine != null) StopCoroutine(danceCoroutine);

            // Update the message
            interactionText.text = "Press E to Dance";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DanceZone")) // Ensure the player enters a designated dance area
            {
                inDanceZone = true;
                interactionUI.SetActive(true); // Show "Press E to Dance"
                interactionText.text = "Press E to Dance"; // Initial message
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("DanceZone"))
            {
                inDanceZone = false;
                interactionUI.SetActive(false); // Hide "Press E to Dance"
                if (isDancing)
                {
                    StopDancing(); // Stop dancing if the player leaves the area
                }
            }
        }
    }
}
