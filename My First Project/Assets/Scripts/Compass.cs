using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform player; // Assign the player's Transform
    public Transform[] taskLocations; // Assign the task locations
    public RectTransform compassArrow; // Assign the RectTransform of the arrow UI element
    public float proximityRadius = 10f; // Radius to detect the nearest task

    private Transform closestTask = null;

    void Update()
    {
        // Find the closest active task
        closestTask = FindClosestTask();

        if (closestTask != null)
        {
            // Calculate the direction to the closest task
            Vector3 directionToTask = closestTask.position - player.position;
            directionToTask.y = 0; // Ignore vertical differences

            // Calculate the angle between the player's forward direction and the task
            float angle = Vector3.SignedAngle(player.forward, directionToTask, Vector3.up);

            // Update the arrow's rotation
            compassArrow.localRotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            // Hide or reset the arrow when no task is nearby
            compassArrow.localRotation = Quaternion.identity;
        }
    }

    Transform FindClosestTask()
    {
        Transform closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform task in taskLocations)
        {
            // Check if the task is active in the scene
            if (!task.gameObject.activeSelf)
                continue;

            // Calculate the distance to the task
            float distance = Vector3.Distance(player.position, task.position);

            if (distance < shortestDistance && distance <= proximityRadius)
            {
                closest = task;
                shortestDistance = distance;
            }
        }

        return closest;
    }
}
