using System.Collections.Generic;
using UnityEngine;

public class RotatingRing : MonoBehaviour
{
    [System.Serializable]
    public class RingSettings
    {
        public Transform ringObject; // The ring to rotate, scale, and float
        public float rotationSpeed = 50f; // Speed of rotation
        public float scalingAmplitude = 0.2f; // Scaling variation (max change in size)
        public float scalingSpeed = 2f; // Speed of the scaling effect
        public float floatAmplitude = 0.2f; // Max upward movement from the base
        public float floatSpeed = 1f; // Speed of the up-and-down movement
        public float floatOffsetStart = 0f; // Starting vertical offset for the floating motion
        public float interactionRange = 5f; // Range for player interaction
    }

    public List<RingSettings> rings = new List<RingSettings>(); // List of rings
    public Transform player; // Reference to the player

    private Dictionary<RingSettings, Vector3> initialScales = new Dictionary<RingSettings, Vector3>();
    private Dictionary<RingSettings, Vector3> initialPositions = new Dictionary<RingSettings, Vector3>();
    private Dictionary<RingSettings, bool> isActive = new Dictionary<RingSettings, bool>();

    void Start()
    {
        foreach (var ring in rings)
        {
            if (ring.ringObject != null)
            {
                initialScales[ring] = ring.ringObject.localScale;
                initialPositions[ring] = ring.ringObject.position + new Vector3(0, ring.floatOffsetStart, 0);
                ring.ringObject.gameObject.SetActive(false);
                isActive[ring] = false;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        foreach (var ring in rings)
        {
            HandleRing(ring);
        }
    }

    void HandleRing(RingSettings ring)
    {
        if (ring.ringObject == null) return;

        float distance = Vector3.Distance(player.position, ring.ringObject.position);

        if (distance <= ring.interactionRange)
        {
            if (!isActive[ring])
            {
                ring.ringObject.gameObject.SetActive(true);
                isActive[ring] = true;
            }

            // Rotate the ring
            ring.ringObject.Rotate(Vector3.up, ring.rotationSpeed * Time.deltaTime);

            // Apply scaling effect
            float scale = Mathf.PingPong(Time.time * ring.scalingSpeed, ring.scalingAmplitude) + 1f;
            ring.ringObject.localScale = initialScales[ring] * scale;

            // Apply up-and-down movement
            float floatOffset = Mathf.Sin(Time.time * ring.floatSpeed) * ring.floatAmplitude;
            ring.ringObject.position = initialPositions[ring] + new Vector3(0, floatOffset, 0);
        }
        else
        {
            if (isActive[ring])
            {
                ring.ringObject.gameObject.SetActive(false);
                isActive[ring] = false;

                // Reset the ring's position and scale
                ring.ringObject.localScale = initialScales[ring];
                ring.ringObject.position = initialPositions[ring];
            }
        }
    }
}
