using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class GameModeManager : MonoBehaviour
    {
        public GameObject[] objectsToDeactivateInVR; // Assign the objects to deactivate in VR 
        public GameObject[] accessBarriers; // Assign the objects to deactivate in VR mode
        public GameObject label; // Assign the objects to deactivate in VR mode
        public GameObject compass;


        public Transform player; // Reference to the player's transform
        public ActivationOfGameStarterNpc scriptToDisableInVR; // Reference to the specific script to disable in VR mode

        private void Start()
        {
            // Retrieve the selected mode
            string selectedMode = PlayerPrefs.GetString("SelectedMode", "Game Mode");
            foreach (var obj2 in accessBarriers)
            {
                obj2.SetActive(true);
            }

            if (selectedMode == "Virtual Reality")
            {
                label.SetActive(true);
                compass.SetActive(false);
                // Deactivate objects
                foreach (var obj in objectsToDeactivateInVR)
                {
                    obj.SetActive(false);
                }

                // Deactivate the specific script
                if (scriptToDisableInVR != null)
                    scriptToDisableInVR.enabled = false;

                // Set player position for VR mode
                player.position = new Vector3(757.615662f, 0.999999225f, 547.593079f); // Adjust to your VR starting position
            }
            else if (selectedMode == "Game Mode")
            {
                label.SetActive(false);

                // Set player position for Game Mode
                player.position = new Vector3(640.748047f, 0.982560635f, 294.887085f); // Adjust to your Game Mode starting position

                // Set player rotation for Game Mode
                player.rotation = Quaternion.Euler(0f, 145f, 0f); // Adjust to your desired rotation (example: facing 90 degrees on the Y axis)
            }
        }
    }
}
