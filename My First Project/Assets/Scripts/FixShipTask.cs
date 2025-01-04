using UnityEngine;
using TMPro;
using System.Collections;

namespace Unity.FantasyKingdom
{
    public class FixShipTask : MonoBehaviour
    {
        public GameObject fixedShipPrefab;    // Reference to the fixed ship prefab
        public TMP_Text taskListText;         // Reference to the task list UI text
        public string taskDescription = "Fix Shinked Pirate Ship"; // Task description
        public GameObject interactionText;    // Text to display interaction message


        private FixPoint[] fixPoints;        // Array of fix points
        private bool taskCompleted = false;

        private void Start()
        {
            // Find all fix points in the ship
            fixPoints = GetComponentsInChildren<FixPoint>();
        }

        private void Update()
        {
            if (!taskCompleted && AreAllPointsFixed())
            {
                ReplaceShip();
                CompleteTask();
            }
        }

        private bool AreAllPointsFixed()
        {
            foreach (FixPoint point in fixPoints)
            {
                if (!point.IsFixed)
                    return false;
            }
            return true;
        }

        private void ReplaceShip()
        {
            if (fixedShipPrefab != null)
            {
                if (interactionText != null)
                {
                    interactionText.SetActive(false);
                    interactionText.GetComponent<TMP_Text>().text = "";
                }
                // Instantiate the fixed ship at the same position and rotation as the broken ship
                Instantiate(fixedShipPrefab, transform.position, transform.rotation);

                // Destroy the broken ship
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Fixed ship prefab is not assigned!");
            }
        }

        private void CompleteTask()
        {
            taskCompleted = true;

            // Update the task in the UI
            if (taskListText != null)
            {
                string completedTask = $"<color=green>{taskDescription}</color>";
                taskListText.text = taskListText.text.Replace(taskDescription, completedTask);
            }

        }
    }
}
