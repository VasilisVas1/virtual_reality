using UnityEngine;

public class ShipSailingEffect : MonoBehaviour
{
    public float speed = 5f;              // Speed of the ship
    public float bobbingAmplitude = 0.5f; // Amplitude of the bobbing motion
    public float bobbingFrequency = 1f;   // Frequency of the bobbing motion
    public float rockingAmplitude = 5f;   // Amplitude of the rocking motion
    public float rockingFrequency = 0.5f; // Frequency of the rocking motion
    public float turnInterval = 10f;      // Time interval before turning 180 degrees

    private Vector3 startPosition;
    private float elapsedTime;
    private float turnTimer;

    void Start()
    {
        startPosition = transform.position;
        elapsedTime = 0f;
        turnTimer = 0f;
    }

    void Update()
    {
        // Move the ship forward continuously based on its current forward direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update elapsed time and turn timer
        elapsedTime += Time.deltaTime;
        turnTimer += Time.deltaTime;

        // Apply bobbing effect
        float bobbingOffset = Mathf.Sin(elapsedTime * bobbingFrequency) * bobbingAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + bobbingOffset, transform.position.z);

        // Apply rocking effect (rotating around the Z-axis)
        float rockingAngle = Mathf.Sin(elapsedTime * rockingFrequency) * rockingAmplitude;
        transform.rotation = Quaternion.Euler(rockingAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Check if it's time to turn 180 degrees
        if (turnTimer >= turnInterval)
        {
            TurnShip();
            turnTimer = 0f; // Reset the turn timer
        }
    }

    void TurnShip()
    {
        // Rotate the ship 180 degrees
        transform.Rotate(0f, 180f, 0f);
    }
}
