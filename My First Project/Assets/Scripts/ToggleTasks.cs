using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class ToggleTasks : MonoBehaviour
    {
 public GameObject uiElement; // Reference to the UI element to toggle

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && uiElement != null)
        {
            if (uiElement.activeSelf) // Check if the UI is currently active
            {
                CloseUIElement();
            }
            else
            {
                OpenUIElement();
            }
        }
    }

    private void OpenUIElement()
    {
        uiElement.SetActive(true); // Activate the UI
    }

    private void CloseUIElement()
    {
        uiElement.SetActive(false); // Deactivate the UI
    }    }
}
