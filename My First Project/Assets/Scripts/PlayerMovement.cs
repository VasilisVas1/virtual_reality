using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 2.0f;
    private float rotationX = 0f;

    void Update()
    {
        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        float moveVertical = Input.GetAxis("Vertical") * speed;

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        transform.position += movement * Time.deltaTime;

        // Camera Rotation
        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        float rotationY = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(0f, rotationY, 0f);
    }
}
