using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class PlayerRespawn : MonoBehaviour
    {
         [SerializeField] private Transform respawnPoint; // Set the respawn point in the Inspector

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
