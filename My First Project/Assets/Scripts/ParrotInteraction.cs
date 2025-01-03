using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class ParrotInteraction : MonoBehaviour
    {
        public GameObject interactionUI;
        public TMP_Text taskListText;
        public string taskDescription = "Find A Parrot with Personality";
        public AudioClip personalitySound;
        public AudioClip normalParrotSound;

        private bool isPersonalityParrot = false;
        private bool playerInRange = false;
        private bool canPickUp = false;
        private bool taskCompleted = false;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            // Find the ParrotManager in the scene
            ParrotManager parrotManager = FindObjectOfType<ParrotManager>();

            if (parrotManager != null)
            {
                // Check if the current parrot is the personality parrot
                isPersonalityParrot = (parrotManager.personalityParrot == gameObject);
            }
            else
            {
                Debug.LogError("ParrotManager not found! Make sure there is a ParrotManager in the scene.");
            }
        }

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (canPickUp)
                {
                    PickUpParrot();
                }
                else
                {
                    InteractWithParrot();
                }
            }
        }

        private void InteractWithParrot()
        {
            if (isPersonalityParrot)
            {
                if (!canPickUp) // Ensure sound and interaction happen only once
                {
                    if (personalitySound != null && !audioSource.isPlaying)
                    {
                        audioSource.clip = personalitySound;
                        audioSource.Play();
                        StartCoroutine(EnablePickUpAfterSound(audioSource.clip.length));
                    }
                }
            }
            else
            {
                if (normalParrotSound != null && !audioSource.isPlaying)
                {
                    audioSource.clip = normalParrotSound;
                    audioSource.Play();
                }
            }
        }

        private System.Collections.IEnumerator EnablePickUpAfterSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isPersonalityParrot)
            {
                canPickUp = true;
                if (interactionUI != null)
                {
                    interactionUI.GetComponent<TMP_Text>().text = "Press E to Pick Up";
                }
            }
        }

        private void PickUpParrot()
        {
            taskCompleted = true;

            // Update task in UI (turn green)
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

        // Make the parrot look at the player
        LookAtPlayer(other.transform);

        if (interactionUI != null)
        {
            interactionUI.SetActive(true);

            // Show appropriate interaction text
            if (canPickUp)
            {
                interactionUI.GetComponent<TMP_Text>().text = "Press E to Pick Up";
            }
            else
            {
                interactionUI.GetComponent<TMP_Text>().text = "Press E to Feed";
            }
        }
    }
}

private void LookAtPlayer(Transform playerTransform)
{
    // Get the direction towards the player
    Vector3 directionToPlayer = playerTransform.position - transform.position;

    // Remove the y component to keep the rotation on the Y axis only
    directionToPlayer.y = 0;

    // Calculate the target rotation to face the player
    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

    // Apply the target rotation directly to the parrot's transform
    transform.rotation = targetRotation;
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
