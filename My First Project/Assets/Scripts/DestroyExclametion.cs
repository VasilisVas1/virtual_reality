using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class DestroyExclametion : MonoBehaviour
    {
        // Key to press for destroying the object
    [SerializeField]
    private KeyCode destroyKey = KeyCode.E;

    // Reference to the object to be destroyed
    private GameObject targetObject;

    // Detect when the player enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is marked for destruction
        if (other.CompareTag("Destroyable"))
        {
            targetObject = other.gameObject;
        }
    }

    // Detect when the player exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Clear the reference when leaving the collider
        if (other.CompareTag("Destroyable"))
        {
            targetObject = null;
        }
    }

    private void Update()
    {
        // Check if the destroy key is pressed and the target object exists
        if (Input.GetKeyDown(destroyKey) && targetObject != null)
        {
            Destroy(targetObject);
            targetObject = null; // Clear reference after destruction
        }
    }
    }
}
