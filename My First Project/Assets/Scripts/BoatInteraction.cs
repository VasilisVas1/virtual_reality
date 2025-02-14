using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatInteraction : MonoBehaviour
{
    public GameObject player; // Αναφορά στον παίκτη
    public GameObject boat; // Αναφορά στη βάρκα
    public GameObject interactionUI; // UI για την αλληλεπίδραση
    public Transform[] portPositions; // Θέσεις λιμανιών
    public Transform[] playerLandPositions; // Θέσεις στις οποίες θα τοποθετηθεί ο παίκτης όταν αποβιβαστεί
    public Camera playerCamera; // Κάμερα παίκτη
    public Camera boatCamera; // Κάμερα βάρκας
    public TMP_Text interactionPrompt; // Κείμενο για την προτροπή αλληλεπίδρασης
    public TMP_Text secondInteractionPrompt; // Δεύτερο κείμενο για επιπλέον οδηγίες

    public float portProximityThreshold = 1f; // Όριο απόστασης για αποβίβαση

    private bool isNearBoat = false; // Αν ο παίκτης είναι κοντά στη βάρκα
    private bool isOnBoat = false; // Αν ο παίκτης είναι πάνω στη βάρκα

    private Vector3 boatStartPosition; // Αρχική θέση της βάρκας
    private Quaternion boatStartRotation; // Αρχικός προσανατολισμός της βάρκας

    void Start()
    {
        // Αποθήκευση της αρχικής θέσης και περιστροφής της βάρκας
        boatStartPosition = boat.transform.position;
        boatStartRotation = boat.transform.rotation;

        // Απενεργοποίηση της κάμερας της βάρκας στην αρχή
        boatCamera.enabled = false;
        boat.GetComponent<BoatController>().enabled = false;

        // Απόκρυψη των αρχικών προτροπών αλληλεπίδρασης
        if (interactionPrompt != null)
        {
            interactionPrompt.text = "";
            interactionPrompt.gameObject.SetActive(false);
        }

        if (secondInteractionPrompt != null)
        {
            secondInteractionPrompt.text = "";
            secondInteractionPrompt.gameObject.SetActive(false);
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
            interactionUI.SetActive(false); // Απόκρυψη UI όταν ο παίκτης δεν είναι κοντά στη βάρκα
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

            // Επαναφορά των UI προτροπών
            if (interactionPrompt != null)
            {
                interactionPrompt.text = "";
                interactionPrompt.gameObject.SetActive(false);
            }

            if (secondInteractionPrompt != null)
            {
                secondInteractionPrompt.text = "";
                secondInteractionPrompt.gameObject.SetActive(false);
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
                secondInteractionPrompt.gameObject.SetActive(false);
                secondInteractionPrompt.text = "";
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
                interactionPrompt.text = "";
                interactionPrompt.gameObject.SetActive(false);
            }

            if (secondInteractionPrompt != null)
            {
                secondInteractionPrompt.text = "";
                secondInteractionPrompt.gameObject.SetActive(false);
            }
        }
    }
}
