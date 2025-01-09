using UnityEngine;
using System.Collections;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class TrapStun : MonoBehaviour
    {
         [SerializeField] private MonoBehaviour playerController; // Reference to the player's controller script
    [SerializeField] private float stunDuration = 15f; // Duration of the stun effect
    [SerializeField] private TextMeshProUGUI stunMessage; // Reference to the TextMeshProUGUI object for the message

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collider belongs to the player
        {
            if (playerController != null)
            {
                StartCoroutine(StunPlayer());
            }
            else
            {
                Debug.LogWarning("PlayerController is not assigned in the Inspector.");
            }
        }
    }

    private IEnumerator StunPlayer()
{
    playerController.enabled = false;

    if (stunMessage != null)
    {
        stunMessage.gameObject.SetActive(true);
        for (float remainingTime = stunDuration; remainingTime > 0; remainingTime--)
        {
            stunMessage.text = $"You are stunned! {remainingTime:F0}s remaining.";
            yield return new WaitForSeconds(1f);
        }
        stunMessage.gameObject.SetActive(false);
    }

    playerController.enabled = true;
}

    }
}
