using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject boat;
    public GameObject interactionUI;     // "Press E to Talk" message
    public Transform[] portPositions; // Array of possible port positions
    public Transform[] playerLandPositions; // Array of land positions for disembarking
    public Camera playerCamera;
    public Camera boatCamera;
    public TMP_Text interactionPrompt; // Reference to the UI Text element
    public TMP_Text secondInteractionPrompt; // Reference to the UI Text element

    public float portProximityThreshold = 1f; // How close the boat needs to be to the port

    private bool isNearBoat = false;
    private bool isOnBoat = false;

    private Vector3 boatStartPosition;
    private Quaternion boatStartRotation;

    void Start()
    {
        boatStartPosition = boat.transform.position;
        boatStartRotation = boat.transform.rotation;

        boatCamera.enabled = false;
        boat.GetComponent<BoatController>().enabled = false;

        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isNearBoat)
        {
            if (isOnBoat)
            {
                interactionUI.SetActive(true);
                // Check if the boat is near any port position
                Transform closestPort = GetClosestPort();
                if (Vector3.Distance(boat.transform.position, closestPort.position) <= portProximityThreshold)
                {
                    secondInteractionPrompt.text = "Press E to Disembark"; 
                }
                else
                {
                    secondInteractionPrompt.text = "";
                }
            }
            else
            {
                interactionUI.SetActive(false);
                interactionPrompt.text = "Press E to ride the boat";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isOnBoat)
                {
                    TryDisembark();
                }
                else
                {
                    BoardBoat();
                }
            }
        }
    }

    private void BoardBoat()
    {
        isOnBoat = true;

        player.SetActive(false);
        boat.GetComponent<BoatController>().enabled = true;

        playerCamera.enabled = false;
        boatCamera.enabled = true;

        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

   private void TryDisembark()
{
    // Find the closest port position
    Transform closestPort = GetClosestPort();

    // Check if the boat is near the closest port position
    if (Vector3.Distance(boat.transform.position, closestPort.position) <= portProximityThreshold)
    {
        // Find the corresponding land position for the closest port
        Transform correspondingLandPosition = playerLandPositions[System.Array.IndexOf(portPositions, closestPort)];

        isOnBoat = false;

        player.SetActive(true);
        player.transform.position = correspondingLandPosition.position;

        // Do not reset the boat's position, so it stays where the player disembarked
        // boat.transform.position = boatStartPosition; // Remove this line

        boat.transform.rotation = boatStartRotation; // You can keep rotation resetting if needed

        // Disable boat controls
        boat.GetComponent<BoatController>().enabled = false;
        
        // Stop the boat's movement immediately
        /*
        Rigidbody boatRb = boat.GetComponent<Rigidbody>();
        boatRb.linearVelocity = Vector3.zero; // Stop linear velocity
        boatRb.angularVelocity = Vector3.zero; // Stop angular velocity
        */

        // Switch cameras
        boatCamera.enabled = false;
        playerCamera.enabled = true;
    }
    else
    {
        Debug.Log("You can only disembark when the boat is at the port!");
    }
}


    private Transform GetClosestPort()
    {
        Transform closestPort = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform port in portPositions)
        {
            float distance = Vector3.Distance(boat.transform.position, port.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPort = port;
            }
        }

        return closestPort;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isNearBoat = true;

            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(true);
                interactionPrompt.text = isOnBoat ? "Press 'E' to disembark" : "Press 'E' to ride the boat";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isNearBoat = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}
