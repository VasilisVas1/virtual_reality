using UnityEngine;
using System.Collections;
using TMPro;


namespace Unity.FantasyKingdom
{
    public class TrapDizziness : MonoBehaviour
    {
    [SerializeField] private float dizzinessDuration = 10f; // Duration of the dizziness effect
    [SerializeField] private Transform playerCamera; // Reference to the player's camera
    [SerializeField] private float rotationSpeed = 50f; // Speed of the rotation effect
    [SerializeField] private TextMeshProUGUI dizzyMessage; // Reference to the TextMeshProUGUI object for the message


    private bool isDizzy = false; // Flag to prevent multiple dizziness effects

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDizzy) // Ensure the collider belongs to the player and the effect isn't already active
        {
            if (playerCamera != null)
            {
                StartCoroutine(ApplyDizziness());
            }
            else
            {
                Debug.LogWarning("PlayerCamera is not assigned in the Inspector.");
            }
        }
    }

    private IEnumerator ApplyDizziness()
    {
        isDizzy = true;
        float elapsed = 0f;
        dizzyMessage.gameObject.SetActive(true);

        while (elapsed < dizzinessDuration)
        {
            // Rotate the camera around the Y-axis
            dizzyMessage.text = $"You stepped on Dizzy Trap";
            playerCamera.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dizzyMessage.gameObject.SetActive(false);
        isDizzy = false; // Reset the flag
    }
    }
}
