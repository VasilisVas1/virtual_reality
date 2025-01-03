using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class ParrotManager : MonoBehaviour
    {
        public GameObject[] parrots; // Array to hold all parrots in the scene
        public GameObject personalityParrot; // Reference to the personality parrot

        private void Awake()
        {
            AssignPersonalityParrot(); // Assign the personality parrot when the game starts
        }

        private void AssignPersonalityParrot()
        {
            parrots = GameObject.FindGameObjectsWithTag("Parrot"); // Find all parrots in the scene

            if (parrots.Length > 0)
            {
                int randomIndex = Random.Range(0, parrots.Length); // Pick a random index
                personalityParrot = parrots[randomIndex]; // Assign the personality parrot
                Debug.Log("The personality parrot has been assigned!");
            }
            else
            {
                Debug.LogError("No parrots found in the scene! The game cannot proceed.");
            }
        }
    }
}
