using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Unity.FantasyKingdom
{
    public class FixPoint : MonoBehaviour
    {
        public GameObject progressBarUI;     // Reference to the progress bar UI
        public GameObject interactionText;  // Text to display interaction message
        public string fixMessage = "Hold E to Fix";
        public AudioClip fixingSound;       // Sound played while fixing

        private AudioSource audioSource;    // Audio source for playing sounds
        private bool playerInRange = false;
        private bool isFixing = false;
        private float fixDuration = 3f;     // Time required to fix this point
        private Slider progressBar;         // Progress bar slider
        public bool IsFixed { get; private set; } = false;

        private void Start()
        {
            if (progressBarUI != null)
            {
                progressBar = progressBarUI.GetComponent<Slider>();
                progressBar.value = 0;
                progressBarUI.SetActive(false);
            }

            // Add or get AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Assign the fixing sound to the AudioSource if provided
            if (fixingSound != null)
            {
                audioSource.clip = fixingSound;
                audioSource.loop = true; // Loop the sound while fixing
            }
        }

        private void Update()
        {
            if (playerInRange && Input.GetKey(KeyCode.E) && !isFixing && !IsFixed)
            {
                StartCoroutine(FixProgress());
            }
        }

        private IEnumerator FixProgress()
        {
            isFixing = true;

            // Play fixing sound
            if (fixingSound != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Show progress bar
            if (progressBarUI != null)
            {
                progressBarUI.SetActive(true);
            }

            float elapsedTime = 0f;

            while (elapsedTime < fixDuration)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    elapsedTime += Time.deltaTime;

                    // Update progress bar
                    if (progressBar != null)
                    {
                        progressBar.value = elapsedTime / fixDuration;
                    }

                    yield return null;
                }
                else
                {
                    // Stop sound if fixing is interrupted
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }

                    // Reset if the player releases the key
                    isFixing = false;
                    if (progressBar != null)
                    {
                        progressBar.value = 0;
                    }
                    yield break;
                }
            }

            // Stop sound after fixing is complete
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Mark the point as fixed and deactivate it
            IsFixed = true;
            isFixing = false;

            // Hide progress bar
            if (progressBarUI != null)
            {
                progressBarUI.SetActive(false);
            }

            DeactivatePoint(); // Deactivate the fix point
        }

        private void DeactivatePoint()
        {
            // Deactivate the interaction and progress UI
            if (interactionText != null)
            {
                interactionText.SetActive(false);
            }
            if (progressBarUI != null)
            {
                progressBarUI.SetActive(false);
            }

            // Deactivate the game object
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !IsFixed)
            {
                playerInRange = true;

                // Show interaction message
                if (interactionText != null)
                {
                    interactionText.SetActive(true);
                    interactionText.GetComponent<TMP_Text>().text = fixMessage;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;

                // Stop sound when the player exits range
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                // Hide progress bar and reset text
                if (progressBarUI != null)
                {
                    progressBarUI.SetActive(false);
                    progressBar.value = 0;
                }

                if (interactionText != null)
                {
                    interactionText.SetActive(false);
                    interactionText.GetComponent<TMP_Text>().text = "";
                }
            }
        }
    }
}
