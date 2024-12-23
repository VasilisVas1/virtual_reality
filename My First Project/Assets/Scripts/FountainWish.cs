using UnityEngine;
using UnityEngine.UI;  // Add this to use the InputField
using TMPro;  // Add this to use TMP_Text

namespace Unity.FantasyKingdom
{
    public class FountainWish : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject interactionUI;   // "Press E to make a wish" message
        [SerializeField] private TMP_Text interactionText;  // TextMeshPro Text for the message
        [SerializeField] private InputField wishInputField;  // Legacy InputField
        [SerializeField] private GameObject coinPrefab; // The coin prefab to toss into the fountain

        private bool playerInRange = false;  // Is the player in range of the fountain
        private bool isMakingWish = false;   // Has the player started making a wish?

        [Header("Player References")]
        public MonoBehaviour playerMovementScript;  // Reference to player's movement script

        void Start()
        {
            interactionUI.SetActive(false);  // Hide interaction UI initially
            wishInputField.gameObject.SetActive(false);  // Hide the input field initially
        }

        void Update()
        {
            // Check for interaction input only if in range
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isMakingWish)
            {
                StartMakingWish();
            }

            // Check for Enter key to submit the wish
            if (isMakingWish && Input.GetKeyDown(KeyCode.Return))
            {
                MakeWish();
            }
        }

        void StartMakingWish()
        {
            isMakingWish = true;
            interactionUI.SetActive(false);  // Hide "Press E to make a wish" message
            wishInputField.gameObject.SetActive(true);  // Show the input field to type the wish

            // Disable player movement while making a wish
            if (playerMovementScript != null) playerMovementScript.enabled = false;

            // Focus the input field for the user to start typing
            wishInputField.Select();  // Select the input field
            wishInputField.ActivateInputField();  // Activate the input field for typing
        }

        void MakeWish()
{
    string playerWish = wishInputField.text;  // Get the player's wish from the input field

    if (!string.IsNullOrEmpty(playerWish))
    {
        // Spawn the coin and toss it into the fountain
        TossCoin();

        // Optionally, clear the input field after the wish
        wishInputField.text = "";

        // End the wish-making process
        EndMakingWish();
    }
    else
    {
        // If the input field is empty, just end the wish-making process
        EndMakingWish();
    }
}


        void TossCoin()
{
    // Instantiate the coin prefab at the player's position
    Vector3 playerPosition = playerMovementScript.transform.position;  // Get the player's position
    GameObject coin = Instantiate(coinPrefab, playerPosition + Vector3.up, Quaternion.identity);  // Add a small offset to make it spawn above the player

    // Apply a force to simulate the toss
    Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();

    if (coinRigidbody != null)
    {
        // Apply an upward force to make it look like it's tossed
        coinRigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);  // Adjust force as needed

        // Optionally, add a horizontal force to make the coin move toward the fountain
        Vector3 directionToFountain = (transform.position - playerPosition).normalized;  // Calculate direction from player to fountain
        coinRigidbody.AddForce(directionToFountain * 3f, ForceMode.Impulse);  // Apply force towards the fountain

        // Apply a torque to make the coin flip (spin around the X and Z axes)
        float flipSpeed = 10f;  // Adjust the speed of the flip as needed
        coinRigidbody.AddTorque(new Vector3(Random.Range(-flipSpeed, flipSpeed), Random.Range(-flipSpeed, flipSpeed), Random.Range(-flipSpeed, flipSpeed)), ForceMode.Impulse);
    }
}




        void EndMakingWish()
        {
            isMakingWish = false;
            wishInputField.gameObject.SetActive(false);  // Hide the input field
            interactionUI.SetActive(true);  // Show "Press E to make a wish" message again

            // Re-enable player movement after making the wish
            if (playerMovementScript != null) playerMovementScript.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Check if the collider is the player
            {
                playerInRange = true;
                interactionUI.SetActive(true);  // Show "Press E to make a wish"
                interactionText.text = "Press E to make a wish";  // Update the text message
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                interactionUI.SetActive(false);  // Hide the interaction message
                wishInputField.gameObject.SetActive(false);  // Hide the input field
                isMakingWish = false;

                // Ensure player can move again if exiting during interaction
                if (playerMovementScript != null) playerMovementScript.enabled = true;
            }
        }
    }
}
