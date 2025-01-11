using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint; // Set the respawn point in the Inspector
        [SerializeField] private Transform respawnPointBoat; // Set the respawn point in the Inspector
        [SerializeField] private GameObject boat; // Reference to the boat GameObject

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the player collides with water
            if (collision.gameObject.CompareTag("Water"))
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            // Move the player to the respawn point
            transform.position = respawnPoint.position;

            // Move the boat to its respawn point
            boat.transform.position = respawnPointBoat.position;

            // Optional: Reset velocity if using Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
