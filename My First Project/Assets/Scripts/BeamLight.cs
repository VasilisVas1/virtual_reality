using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class BeamLight : MonoBehaviour
    {
        public Transform npc; // Reference to the NPC
        public float beamHeight = 10f; // Height of the beam
        public Color beamColor = new Color(0, 0, 1, 0.5f); // Blue color with 50% transparency
        public float beamWidth = 0.5f; // Width of the beam
        public Material beamMaterial; // Assign your transparent material here

        private LineRenderer lineRenderer;

        void Start()
        {
            // Get or add LineRenderer component
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            // Assign the transparent material to the LineRenderer
            if (beamMaterial != null)
            {
                lineRenderer.material = beamMaterial;
            }
            else
            {
                Debug.LogWarning("Beam Material is not assigned. Please assign a material with transparency.");
            }

            // Set line renderer properties
            lineRenderer.startWidth = beamWidth;
            lineRenderer.endWidth = beamWidth;

            // Apply the beam color with transparency
            lineRenderer.startColor = beamColor;
            lineRenderer.endColor = beamColor;

            // Set the beam's start position at the NPC's position
            Vector3 beamStartPosition = npc.position;

            // Set the beam's end position above the NPC
            Vector3 beamEndPosition = new Vector3(npc.position.x, npc.position.y + beamHeight, npc.position.z);

            // Set the line renderer points to form the beam
            lineRenderer.SetPosition(0, beamStartPosition);
            lineRenderer.SetPosition(1, beamEndPosition);
        }
    }
}
