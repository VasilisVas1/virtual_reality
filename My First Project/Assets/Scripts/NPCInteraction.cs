using System.Collections;
using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class NPCInteraction : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI;     // "Press E to Talk" message
        [SerializeField] private TMP_Text interactionText; // Text for "Press E to Talk to Name"
        [SerializeField] private TMP_Text dialogueText;    // For TextMeshPro Dialogue
        [SerializeField] private RectTransform bubblePanel; // The panel that will act as the chat bubble
        [SerializeField] private Vector3 bubbleOffset = new Vector3(0, 2, 0); // Offset for the bubble above the NPC

        [Header("NPC Info")]
        public string npcName;              // Name of the NPC

        [Header("Dialogue Settings")]
        public string[] npcSentences;        // Array of sentences for the NPC
        private int currentSentenceIndex = 0;

        private bool playerInRange = false;  // Is the player in range of the NPC
        private bool isTalking = false;      // Is the NPC currently talking

        [Header("Player References")]
        public MonoBehaviour playerMovementScript;  // Reference to player's movement script

        [Header("Typewriter Effect")]
        public float typingSpeed = 0.05f; // Delay between each character

        private Coroutine typingCoroutine; // Reference to the current typing coroutine

        void Start()
        {
            interactionUI.SetActive(false);  // Hide "Press E to Talk" initially
            dialogueText.gameObject.SetActive(false); // Hide dialogue box
            bubblePanel.gameObject.SetActive(false); // Hide the bubble initially
        }

        void Update()
        {
            // Check for interaction input
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (!isTalking)
                {
                    StartTalking();
                }
                else
                {
                    DisplayNextSentence();
                }
            }
        }

        void StartTalking()
        {
            isTalking = true;
            interactionUI.SetActive(false);          // Hide "Press E to Talk"
            bubblePanel.gameObject.SetActive(true); // Show dialogue bubble
            currentSentenceIndex = 0;                // Start from first sentence

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(npcSentences[currentSentenceIndex]));
            dialogueText.gameObject.SetActive(true); // Show dialogue box

            // Adjust bubble size based on text content
            AdjustBubbleSize();

            // Position the bubble above the NPC
            bubblePanel.position = transform.position + bubbleOffset;

            // Disable player movement
            if (playerMovementScript != null) playerMovementScript.enabled = false;
        }

        void DisplayNextSentence()
        {
            currentSentenceIndex++;

            if (currentSentenceIndex < npcSentences.Length)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                typingCoroutine = StartCoroutine(TypeSentence(npcSentences[currentSentenceIndex]));
                AdjustBubbleSize();
            }
            else
            {
                EndDialogue();
            }
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = ""; // Clear the current text

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        void AdjustBubbleSize()
        {
            // Set a fixed width for the text
            float fixedWidth = 300f; // Adjust this value to your desired panel width

            dialogueText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fixedWidth);

            // Force the text to update its layout
            dialogueText.ForceMeshUpdate();

            // Get the preferred height based on the fixed width
            float textHeight = dialogueText.preferredHeight;

            // Set the size of the bubble panel
            bubblePanel.sizeDelta = new Vector2(fixedWidth + 20, textHeight + 20); // Add padding
        }

        void EndDialogue()
        {
            isTalking = false;
            bubblePanel.gameObject.SetActive(false); // Hide dialogue bubble
            interactionUI.SetActive(true);            // Show "Press E to Talk" again

            // Enable player movement
            if (playerMovementScript != null) playerMovementScript.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the collider is the player
            {
                playerInRange = true;
                interactionUI.SetActive(true); // Show "Press E to Talk"
                interactionText.text = $"Press E to Talk to {npcName}"; // Update the text with NPC name
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionUI.SetActive(false);   // Hide "Press E to Talk"
                bubblePanel.gameObject.SetActive(false); // Hide dialogue bubble
                isTalking = false;

                // Ensure player can move again if exiting during interaction
                if (playerMovementScript != null) playerMovementScript.enabled = true;
            }
        }
    }
}
