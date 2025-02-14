using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class ParrotManager : MonoBehaviour
    {
        public GameObject personalityParrot; // Το σταθερό παπαγαλάκι προσωπικότητας
        public Transform[] startPositions;   // Πιθανές αρχικές θέσεις

        private void Awake()
        {
            AssignPersonalityParrotPosition();// Καλεί τη μέθοδο για την τοποθέτηση του παπαγάλου σε μια αρχική θέση
        }

        private void AssignPersonalityParrotPosition()
        {
            // Έλεγχος αν ο παπαγάλος ή οι θέσεις δεν έχουν οριστεί σωστά
            if (personalityParrot == null || startPositions.Length < 3)
            {
                Debug.LogError("Please assign the Personality Parrot and at least 3 start positions in the ParrotManager!");
                return;
            }

            // Επιλογή μιας τυχαίας αρχικής θέσης για τον Παπαγάλο Προσωπικότητας
            int randomPositionIndex = Random.Range(0, startPositions.Length);
            personalityParrot.transform.position = startPositions[randomPositionIndex].position;

            Debug.Log($"Personality Parrot is at {startPositions[randomPositionIndex].position}.");
        }
    }
}
