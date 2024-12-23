using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    public float speed = 500f; // Forward and backward speed
    public float rotationSpeed = 80f; // Turning speed

    private Rigidbody rb;

    void Start()
{
    rb = GetComponent<Rigidbody>();
    rb.constraints = RigidbodyConstraints.FreezeRotation; // Freeze all rotations
    rb.angularDamping = 5f; // Add angular drag to prevent unwanted rotation
}

void FixedUpdate()
{
    // Ensure boat stays at or above water level
    float waterLevel = -12f; // Replace with your water's y-coordinate
    if (transform.position.y < waterLevel||transform.position.y>waterLevel)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Stop downward movement
        transform.position = new Vector3(transform.position.x, waterLevel, transform.position.z);
    }

    // Forward and backward movement
    float moveInput = Input.GetAxis("Vertical");
    Vector3 moveDirection = transform.forward * moveInput * speed * Time.fixedDeltaTime;
    rb.MovePosition(rb.position + moveDirection);

    // Rotation
    float turnInput = Input.GetAxis("Horizontal");
    float turnAmount = turnInput * rotationSpeed * Time.fixedDeltaTime;
    Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
    rb.MoveRotation(rb.rotation * turnRotation);
}


}
