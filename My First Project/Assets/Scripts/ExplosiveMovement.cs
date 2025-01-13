using UnityEngine;

public class ExplosiveMovement : MonoBehaviour
{
    public GameObject explosivePrefab; // Prefab of the explosive item
    public float throwForce = 10f; // Force applied to throw the explosive (although it's now at the feet, you can adjust if needed)
    public float explosionForce = 500f; // Force of the explosion
    public float explosionRadius = 5f; // Radius of the explosion
    public int maxUses = 3; // Maximum number of uses

    private int remainingUses;
    private Rigidbody playerRigidbody;
    public GameObject pauseMenuUI;
    public GameObject settingsPanel;
    public GameObject helpMenu;

    // Offset for the grenade spawn position relative to player's feet (adjust as needed)
    public Vector3 throwOffset = new Vector3(0, -1, 0); 

    void Start()
    {
        remainingUses = maxUses;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
         if (pauseMenuUI.activeSelf || settingsPanel.activeSelf || helpMenu.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G) && remainingUses > 0)
        {
            ThrowExplosive();
        }
    }

    void ThrowExplosive()
{
    remainingUses--;

    // Calculate the position for the bomb to appear slightly behind the player
    Vector3 feetPosition = transform.position + transform.forward * -3 + throwOffset;

    // Instantiate the explosive at the adjusted position
    GameObject explosive = Instantiate(explosivePrefab, feetPosition, Quaternion.identity);

    // Add a small random impulse force to simulate the "throw" effect (optional)
    Rigidbody explosiveRb = explosive.GetComponent<Rigidbody>();
    if (explosiveRb != null)
    {
        explosiveRb.AddForce(Vector3.up * throwForce, ForceMode.Impulse); // Small upward force when thrown
    }

    // Immediately simulate the explosion (no delay)
    Explode(explosive);
}

public AudioClip explosionSound; // The explosion sound clip

void Explode(GameObject explosive)
{
    // Play the explosion sound at the explosion position
    AudioSource.PlayClipAtPoint(explosionSound, explosive.transform.position);

    // Explosion effect
    Vector3 explosionPosition = explosive.transform.position;
    Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

    foreach (Collider nearbyObject in colliders)
    {
        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);

            // If it's the player, apply a forward force (based on the player's facing direction)
            if (rb == playerRigidbody)
            {
                // Apply force in the direction the player is facing (forward direction)
                Vector3 forwardDirection = playerRigidbody.transform.forward;
                playerRigidbody.AddForce(forwardDirection * explosionForce, ForceMode.Impulse);
            }
        }
    }

    // Destroy the explosive object immediately after the explosion
    Destroy(explosive);
}



    void OnGUI()
{
    // Create a GUIStyle to modify font size
    GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
    guiStyle.fontSize = 24; // Set the font size to a larger value

    // Calculate position relative to the bottom-left corner
    float xPosition = 20f; // Margin from the left edge
    float yPosition = Screen.height - 50f; // Margin from the bottom edge

    // Position the label dynamically based on screen size
    GUI.Label(new Rect(xPosition, yPosition, 600, 30), "Explosives Remaining (Press G to use): " + remainingUses, guiStyle);
}


}
