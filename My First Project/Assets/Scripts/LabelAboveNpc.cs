using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class LabelAboveNpc : MonoBehaviour
    {
        [Header("Text Settings")]
    [SerializeField] private string displayText = "START GAME";
    [SerializeField] private Vector3 textOffset = new Vector3(0, 2.5f, 0); // Offset above the NPC
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private int fontSize = 24;

    private TextMeshPro textMeshPro;

    void Start()
    {
        // Create a new GameObject for the TextMeshPro
        GameObject textObject = new GameObject("GameStarterText");
        textObject.transform.SetParent(transform);

        // Position the text above the NPC
        textObject.transform.localPosition = textOffset;

        // Add the TextMeshPro component
        textMeshPro = textObject.AddComponent<TextMeshPro>();
        textMeshPro.text = displayText;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = textColor;
        textMeshPro.fontSize = fontSize;

        // Optional: Add a billboard effect to always face the camera
        textObject.AddComponent<Billboard>();
    }
}

// Optional Billboard Script for Text to Face Camera
public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }

    }
}
