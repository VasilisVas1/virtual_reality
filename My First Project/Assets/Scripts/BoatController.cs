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
    rb.angularDamping = 10f; // Increase angular damping to slow unwanted rotations
    rb.linearDamping = 2f; // Increase linear drag to slow down unwanted linear movement
    rb.angularDamping = 2f; // Increase angular drag to slow down unwanted rotation
}


void FixedUpdate()
{
    // Ensure boat stays at or above water level
    float waterLevel = -12f; // Replace with your water's y-coordinate
    if (transform.position.y < waterLevel || transform.position.y > waterLevel)
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

    // Apply counteracting force to reduce drifting
    if (moveInput == 0) // Only apply counteracting force if there's no movement input
    {
        // Apply a small force to counteract drifting
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); // Smoothly reduce velocity
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); // Smoothly reduce angular velocity
    }
}



}
