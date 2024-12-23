using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BoatInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject boat;
    public GameObject interactionUI;     // "Press E to Talk" message
    public Transform portPosition; // The port's position
    public Transform playerLandPosition; // Where the player lands after disembarking
    public Camera playerCamera;
    public Camera boatCamera;
    public TMP_Text interactionPrompt; // Reference to the UI Text element
    public TMP_Text secondinteractionPrompt; // Reference to the UI Text element

    public float portProximityThreshold = 1f; // How close the boat needs to be to the port
    public GameObject disembarkCollider;

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
            if (Vector3.Distance(boat.transform.position, portPosition.position) <= portProximityThreshold)
            {
                secondinteractionPrompt.text = "Press E to Disembark"; 
            }
            else
            {
                secondinteractionPrompt.text = "";
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
        // Check if the boat is near the port
        if (Vector3.Distance(boat.transform.position, portPosition.position) <= portProximityThreshold)
        {
            isOnBoat = false;

            player.SetActive(true);
            player.transform.position = playerLandPosition.position;

            boat.transform.position = boatStartPosition;
            boat.transform.rotation = boatStartRotation;

            // Disable boat controls
            boat.GetComponent<BoatController>().enabled = false;

            // Switch cameras
            boatCamera.enabled = false;
            playerCamera.enabled = true;
        }
        else
        {
            Debug.Log("You can only disembark when the boat is at the port!");
        }
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
