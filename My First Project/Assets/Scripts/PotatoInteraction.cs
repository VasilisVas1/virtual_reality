using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class PotatoInteraction : MonoBehaviour
    {
public GameObject interactionUI;  // "Press E to Pick Up" message
    public TMP_Text taskListText;     // Reference to the task list UI text
    public string taskDescription = "Find A Perfectly Round Potato"; // Task description

    private bool playerInRange = false; // Check if player is near the potato
    private bool taskCompleted = false; // Check if task is done

    private void Start()
    {
        interactionUI.SetActive(false); // Hide interaction message at the start
    }

    private void Update()
    {
        if (playerInRange && !taskCompleted && Input.GetKeyDown(KeyCode.E))
        {
            PickUpPotato();
        }
    }

    private void PickUpPotato()
    {
        taskCompleted = true;

        // Update the task in the UI (turn it green)
        string completedTask = $"<color=green>{taskDescription}</color>";
        taskListText.text = taskListText.text.Replace(taskDescription, completedTask);

        // Hide the potato
        gameObject.SetActive(false);

        // Hide interaction message
        interactionUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!taskCompleted)
                interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionUI.SetActive(false);
        }
    }
    }
}
