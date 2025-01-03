using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class SingingFishManager : MonoBehaviour
    {
        [Header("Fish Settings")]
        public SingingFishInteraction[] fishes; // Array of all fish scripts
        private SingingFishInteraction singingFish; // The randomly chosen singing fish

        [Header("UI Elements")]
        [SerializeField] private TMP_Text taskListText; // Reference to the task list UI text
        public string taskDescription = "A Rare Singing Fish"; // Task description

        [Header("Audio Settings")]
        [SerializeField] private AudioClip singingFishAudioClip; // The audio resource for the singing fish

       
        private void Start()
        {
            AssignSingingFish();
        }

        private void AssignSingingFish()
        {
            if (fishes == null || fishes.Length == 0)
            {
                Debug.LogError("No fishes assigned in the SingingFishManager!");
                return;
            }

            if (taskListText == null)
            {
                Debug.LogError("Task list text is not assigned in the SingingFishManager!");
                return;
            }

            // Clear audio for all fishes
            foreach (var fish in fishes)
            {
                var fishAudioSource = fish.GetComponent<AudioSource>();
                if (fishAudioSource != null)
                {
                    fishAudioSource.clip = null;
                    fishAudioSource.Stop();
                }
            }

            // Randomly choose a singing fish
            singingFish = fishes[Random.Range(0, fishes.Length)];

            // Assign the audio resource to the chosen singing fish
            var audioSource = singingFish.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = singingFish.gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = singingFishAudioClip;
            audioSource.loop = true; // Optional: Set looping if required
            audioSource.playOnAwake = false; // Optional: Prevent autoplay
            audioSource.Play();

            // Update the task list UI
            singingFish.SetAsSingingFish(taskListText, taskDescription);

            Debug.Log($"Singing fish assigned: {singingFish.gameObject.name}");
        }
    }
}
