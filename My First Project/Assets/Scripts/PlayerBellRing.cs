using System.Collections;
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class PlayerBellRing : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI;     // "Press E to Ring Bell" message
        [SerializeField] private TMP_Text interactionText; // Text for "Press E to Ring Bell"

        [Header("Sound Settings")]
        public AudioSource bellAudioSource;  // AudioSource component attached to the bell
        public AudioClip bellSound;          // The bell sound clip to play

        private bool inBellZone = false;     // Is the player in the bell zone

        void Start()
        {
            interactionUI.SetActive(false); // Hide "Press E to Ring Bell" initially
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && inBellZone)
            {
                RingBell(); // Play the bell sound when "E" is pressed
            }
        }

        void RingBell()
        {
            if (bellAudioSource != null && bellSound != null)
            {
                bellAudioSource.PlayOneShot(bellSound); // Play the bell sound
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BellZone")) // Ensure the player enters the designated bell zone
            {
                inBellZone = true;
                interactionUI.SetActive(true); // Show "Press E to Ring Bell"
                interactionText.text = "Press E to Ring Bell"; // Initial message
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BellZone"))
            {
                inBellZone = false;
                interactionUI.SetActive(false); // Hide "Press E to Ring Bell"
            }
        }
    }
}
