using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class ActivationOfGameStarterNpc : MonoBehaviour
    {
        [Header("Objects to Activate")]
        [SerializeField] private GameObject gameStarterNPC;
        [SerializeField] private GameObject beamLight;
        [SerializeField] private GameObject positionOfBeamLight;

        [SerializeField] private GameObject beamLight2;
        [SerializeField] private GameObject positionOfBeamLight2;

        [Header("Settings")]
        [SerializeField] private string playerTag = "Player"; // Tag to identify the player
        [SerializeField] private KeyCode activationKey = KeyCode.E; // Key to activate objects

        [Header("NPC Target Position")]
        [SerializeField] private Vector3 targetPosition; // Target position for the NPC to move to

        private bool isPlayerInRange = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                isPlayerInRange = false;
            }
        }

        private void Update()
        {
            if (isPlayerInRange && Input.GetKeyDown(activationKey))
            {
                ActivateObjects();
            }
        }

        private void ActivateObjects()
        {
            // Instantly move the NPC to the target position
            if (gameStarterNPC != null)
            {
                gameStarterNPC.transform.position = targetPosition;
            }

            if (beamLight != null) beamLight.SetActive(true);
            if (positionOfBeamLight != null) positionOfBeamLight.SetActive(true);

            if (positionOfBeamLight2 != null) positionOfBeamLight2.SetActive(false);
            if (beamLight2 != null) beamLight2.SetActive(false);
        }
    }
}
