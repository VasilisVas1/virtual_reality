using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class JerryTask : MonoBehaviour
    {
        public GameObject[] hidingPoints;    // Points where Jerry can run
        public TMP_Text taskListText;
        public string taskDescription = "Catch Jerry the Goblin";
        public GameObject interactionUI;

        public float sightRange = 10f;        // How far Jerry can see
        public float sightAngle = 45f;        // Angle of Jerry's field of view
        public float moveSpeed = 3f;          // Movement speed of Jerry
        public float catchRange = 2f;         // Range to catch Jerry
        public float offsetHeight = 1.5f;    // Height from which Jerry's FOV starts (adjust this value)

        private bool taskCompleted = false;
        private bool playerInRange = false;
        private bool isNearJerry = false;
        private bool isRunningAway = false;
        private int currentHidingPointIndex = 0;

        private GameObject jerry;            // Reference to Jerry the goblin
        private Animator jerryAnimator;      // Animator for Jerry's movement
        private Transform playerTransform;   // Reference to the player's transform

        private void Start()
        {
            jerry = gameObject;
            jerryAnimator = jerry.GetComponent<Animator>();
            playerTransform = Camera.main.transform; // Assuming the camera is parented to the player
        }

        private void Update()
        {
            if (taskCompleted) return;

            if (playerInRange && !isRunningAway && !taskCompleted)
            {
                if (CanSeePlayer())
                {
                    // Jerry sees the player and runs away to the next point
                    isRunningAway = true;
                    StartCoroutine(JerryRunsAway());
                }
            }

            if (isNearJerry && Input.GetKeyDown(KeyCode.E))
            {
                CatchJerry();
            }
        }

        private bool CanSeePlayer()
        {
            // Check if the player is within Jerry's FOV
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // Adjust the height by adding the offset
            directionToPlayer.y = offsetHeight;

            // Check if the player is within the sight range
            if (directionToPlayer.magnitude < sightRange)
            {
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                // Check if the player is within the sight angle
                if (angle < sightAngle)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * offsetHeight, directionToPlayer.normalized, out hit, sightRange))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            return true; // Player is visible
                        }
                    }
                }
            }

            return false; // Player is not in sight
        }

        private void CatchJerry()
        {
            taskCompleted = true;

            // Update task in UI
            if (taskListText != null)
            {
                string completedTask = $"<color=green>{taskDescription}</color>";
                taskListText.text = taskListText.text.Replace(taskDescription, completedTask);
            }

            // Hide Jerry and interaction UI
            if (interactionUI != null)
            {
                interactionUI.SetActive(false);
            }
            jerry.SetActive(false); // Jerry disappears when caught
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !taskCompleted)
            {
                playerInRange = true;

                // Show interaction UI
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
                isNearJerry = false;

                // Hide interaction UI
                if (interactionUI != null)
                {
                    interactionUI.SetActive(false);
                }
            }
        }

        private System.Collections.IEnumerator JerryRunsAway()
{
    while (isRunningAway)
    {
        // Move Jerry to the next hiding point
        if (currentHidingPointIndex < hidingPoints.Length)
        {
            Vector3 targetPosition = hidingPoints[currentHidingPointIndex].transform.position;
            while (Vector3.Distance(jerry.transform.position, targetPosition) > 0.1f)
            {
                jerry.transform.position = Vector3.MoveTowards(jerry.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Update hiding point index for next movement
            currentHidingPointIndex++;
            
            // If we reach the last hiding point, loop back to the first one
            if (currentHidingPointIndex >= hidingPoints.Length)
            {
                currentHidingPointIndex = 0; // Reset to the first hiding point
            }

            yield return new WaitForSeconds(2f); // Wait before Jerry runs again
        }
    }
}


        private void OnDrawGizmos()
        {
            // Draw the sight range of Jerry
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            // Draw the field of view cone
            Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle, 0) * transform.forward * sightRange;
            Vector3 rightBoundary = Quaternion.Euler(0, sightAngle, 0) * transform.forward * sightRange;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * offsetHeight, transform.position + Vector3.up * offsetHeight + leftBoundary);
            Gizmos.DrawLine(transform.position + Vector3.up * offsetHeight, transform.position + Vector3.up * offsetHeight + rightBoundary);
        }
    }
}
