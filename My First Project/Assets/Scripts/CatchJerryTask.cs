using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class CatchJerryTask : MonoBehaviour
    {
        public TMP_Text taskListText;        
        public string taskDescription = "Catch Jerry the Goblin"; 
        public GameObject interactionUI;    
        public GameObject jerryGameObject;  

        private bool playerInRange = false; 
        private bool taskCompleted = false; 

        public GameStarterNPC gameStarterNPC; 


        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && !taskCompleted)
            {
                CatchJerry();
            }
        }

        private void CatchJerry()
        {
            taskCompleted = true;

            if (taskListText != null)
            {
                string completedTask = $"<color=green>{taskDescription}</color>";
                taskListText.text = taskListText.text.Replace(taskDescription, completedTask);

                gameStarterNPC.TaskCompleted(5);

            }

            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }

            if (jerryGameObject != null)
            {
                jerryGameObject.SetActive(false);
            }

            Debug.Log("Jerry has been caught!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !taskCompleted)
            {
                playerInRange = true;

                // Display interaction UI
                if (interactionUI != null)
                {
                    interactionUI.SetActive(true);
                    interactionUI.GetComponent<TMP_Text>().text = "Press E to Catch Jerry";
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;

                // Hide interaction UI
                if (interactionUI != null)
                {
                    interactionUI.SetActive(false);
                }
            }
        }
    }
}
