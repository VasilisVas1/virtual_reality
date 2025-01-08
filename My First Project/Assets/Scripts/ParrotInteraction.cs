using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class ParrotInteraction : MonoBehaviour
    {
        public GameObject interactionUI;
        public TMP_Text taskListText;
        public string taskDescription = "Find the Personality Parrot";
        public AudioClip personalitySound;
        public AudioClip normalParrotSound;

        public bool isPersonalityParrot = false; // Set in the Inspector for the Personality Parrot

        private bool playerInRange = false;
        private bool canPickUp = false;
        private bool taskCompleted = false;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                InteractWithParrot();
            }
        }

        private void InteractWithParrot()
        {
            if (isPersonalityParrot)
            {
                if (!canPickUp) // Play the personality sound and allow pickup
                {
                    PlaySound(personalitySound);
                    StartCoroutine(EnablePickUpAfterSound(audioSource.clip.length));
                }
                else
                {
                    PickUpParrot();
                }
            }
            else
            {
                // Regular parrot interaction
                PlaySound(normalParrotSound);
                Debug.Log("This is a normal parrot.");
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null && audioSource != null && !audioSource.isPlaying)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private System.Collections.IEnumerator EnablePickUpAfterSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            canPickUp = true;
            if (interactionUI != null)
            {
                interactionUI.GetComponent<TMP_Text>().text = "Press E to Pick Up";
            }
        }

        private void PickUpParrot()
        {
            taskCompleted = true;

            // Update task in UI
            if (taskListText != null)
            {
                string completedTask = $"<color=green>{taskDescription}</color>";
                taskListText.text = taskListText.text.Replace(taskDescription, completedTask);
            }

            // Hide the parrot and interaction UI
            gameObject.SetActive(false);
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;

                if (interactionUI != null)
                {
                    interactionUI.SetActive(true);
                    interactionUI.GetComponent<TMP_Text>().text = isPersonalityParrot && canPickUp
                        ? "Press E to Pick Up"
                        : "Press E to Feed";
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;

                if (interactionUI != null)
                {
                    interactionUI.SetActive(false);
                }
            }
        }
    }
}
