using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class ParrotManager : MonoBehaviour
    {
        public GameObject personalityParrot; // The fixed Personality Parrot
        public Transform[] startPositions;   // Possible starting positions

        private void Awake()
        {
            AssignPersonalityParrotPosition();
        }

        private void AssignPersonalityParrotPosition()
        {
            if (personalityParrot == null || startPositions.Length < 3)
            {
                Debug.LogError("Please assign the Personality Parrot and at least 3 start positions in the ParrotManager!");
                return;
            }

            // Randomly choose a starting position for the Personality Parrot
            int randomPositionIndex = Random.Range(0, startPositions.Length);
            personalityParrot.transform.position = startPositions[randomPositionIndex].position;

            Debug.Log($"Personality Parrot is at {startPositions[randomPositionIndex].position}.");
        }
    }
}
