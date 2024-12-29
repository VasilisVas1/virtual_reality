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
        interactionPrompt.text = ""; // Clear any initial text
        interactionPrompt.gameObject.SetActive(false); // Hide the prompt initially
    }

    if (secondInteractionPrompt != null)
    {
        secondInteractionPrompt.text = ""; // Clear any initial text
        secondInteractionPrompt.gameObject.SetActive(false); // Hide the second prompt initially
    }
}


    void Update()
{
    if (isNearBoat)
    {
        interactionUI.SetActive(true);

        if (isOnBoat)
        {
            Transform closestPort = GetClosestPort();
            if (Vector3.Distance(boat.transform.position, closestPort.position) <= portProximityThreshold)
            {
                secondInteractionPrompt.gameObject.SetActive(true);
                secondInteractionPrompt.text = "Press E to Disembark";
            }
            else
            {
                secondInteractionPrompt.gameObject.SetActive(false);
                secondInteractionPrompt.text = "";
            }
        }
        else
        {
            interactionPrompt.gameObject.SetActive(true);
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
    else
    {
        interactionUI.SetActive(false); // Hide the UI when not near the boat
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
    Transform closestPort = GetClosestPort();

    if (Vector3.Distance(boat.transform.position, closestPort.position) <= portProximityThreshold)
    {
        Transform correspondingLandPosition = playerLandPositions[System.Array.IndexOf(portPositions, closestPort)];

        isOnBoat = false;
        isNearBoat = false;

        player.SetActive(true);
        player.transform.position = correspondingLandPosition.position;

        boat.transform.rotation = boatStartRotation;

        boat.GetComponent<BoatController>().enabled = false;

        Rigidbody boatRb = boat.GetComponent<Rigidbody>();
        boatRb.linearVelocity = Vector3.zero;
        boatRb.angularVelocity = Vector3.zero;

        boatCamera.enabled = false;
        playerCamera.enabled = true;

        // Reset UI prompts
        if (interactionPrompt != null)
        {
            interactionPrompt.text = ""; // Clear the text
            interactionPrompt.gameObject.SetActive(false); // Hide the prompt
        }

        if (secondInteractionPrompt != null)
        {
            secondInteractionPrompt.text = ""; // Clear any lingering text
            secondInteractionPrompt.gameObject.SetActive(false); // Hide it
        }
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
            interactionPrompt.text = isOnBoat ? "Press E to disembark" : "Press E to ride the boat";
        }

        if (secondInteractionPrompt != null)
        {
            secondInteractionPrompt.gameObject.SetActive(false); // Hide the second prompt
            secondInteractionPrompt.text = ""; // Clear any lingering text
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
            interactionPrompt.text = ""; // Clear the text
            interactionPrompt.gameObject.SetActive(false); // Hide it
        }

        if (secondInteractionPrompt != null)
        {
            secondInteractionPrompt.text = ""; // Clear any lingering text
            secondInteractionPrompt.gameObject.SetActive(false); // Hide it
        }
    }
}



}
