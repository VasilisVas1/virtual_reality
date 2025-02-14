using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    public float speed = 500f; //Ταχύτητα κίνησης
    public float rotationSpeed = 80f; // Ταχύτητα στροφής

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Πάγωμα όλων των περιστροφών
        rb.angularDamping = 10f; // Αυξημένη γωνιακή απόσβεση για μείωση ανεπιθύμητων περιστροφών
        rb.linearDamping = 2f; // Αυξημένη γραμμική αντίσταση για μείωση ανεπιθύμητης κίνησης
        rb.angularDamping = 2f; // Αυξημένη γωνιακή αντίσταση για μείωση ανεπιθύμητων περιστροφών
    }

    void FixedUpdate()
    {
        // Εξασφάλιση ότι η βάρκα παραμένει στην επιφάνεια του νερού
        float waterLevel = -12f; // Αντικαταστήστε με τη σωστή y-συντεταγμένη του νερού
        if (transform.position.y < waterLevel || transform.position.y > waterLevel)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Σταμάτημα της καθοδικής κίνησης
            transform.position = new Vector3(transform.position.x, waterLevel, transform.position.z);
        }

        // Κίνηση προς τα εμπρός και πίσω
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Περιστροφή της βάρκας
        float turnInput = Input.GetAxis("Horizontal");
        float turnAmount = turnInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Εφαρμογή αντίθετης δύναμης για μείωση της ολίσθησης
        if (moveInput == 0) // Εφαρμόζεται μόνο αν δεν υπάρχει εισαγωγή κίνησης
        {
            // Εφαρμογή μικρής δύναμης για μείωση της ολίσθησης
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); // Ομαλή μείωση της ταχύτητας
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); // Ομαλή μείωση της γωνιακής ταχύτητας
        }
    }
}
