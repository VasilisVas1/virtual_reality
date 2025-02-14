using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint; // Ορισμός του σημείου επαναφοράς του παίκτη στο Inspector
        [SerializeField] private Transform respawnPointBoat; // Ορισμός του σημείου επαναφοράς της βάρκας στο Inspector
        [SerializeField] private GameObject boat; // Αναφορά στο GameObject της βάρκας

        private void OnCollisionEnter(Collision collision)
        {
            // Έλεγχος αν ο παίκτης συγκρούεται με το νερό
            if (collision.gameObject.CompareTag("Water"))
            {
                Respawn(); // Κλήση της συνάρτησης επαναφοράς
            }
        }

        private void Respawn()
        {
            // Μετακίνηση του παίκτη στο σημείο επαναφοράς
            transform.position = respawnPoint.position;

            // Μετακίνηση της βάρκας στο σημείο επαναφοράς της
            boat.transform.position = respawnPointBoat.position;

            // Προαιρετικό: Επαναφορά της ταχύτητας αν χρησιμοποιείται Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Μηδενισμός της γραμμικής ταχύτητας
                rb.angularVelocity = Vector3.zero; // Μηδενισμός της γωνιακής ταχύτητας
            }
        }
    }
}
