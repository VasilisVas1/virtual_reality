using UnityEngine;
using TMPro;

namespace Unity.FantasyKingdom
{
    public class TownLabel : MonoBehaviour
    {
        [Header("Text Settings")]
        [SerializeField] private string displayText = "START GAME";
        [SerializeField] private Vector3 textOffset = new Vector3(0, 2.5f, 0); // Offset above the NPC
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private int fontSize = 24;
        [SerializeField] private Vector2 rectSize = new Vector2(200, 50); // Size of the RectTransform

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

            // Adjust the RectTransform of the TextMeshPro
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = rectSize;
                rectTransform.pivot = new Vector2(0.5f, 0.5f); // Center pivot
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = Vector2.zero;
            }

            // Optional: Add a billboard effect to always face the camera
            textObject.AddComponent<Billboard>();
        }
    }
}
