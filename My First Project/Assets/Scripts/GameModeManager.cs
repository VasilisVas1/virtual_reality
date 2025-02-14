using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class GameModeManager : MonoBehaviour
    {
        public GameObject[] objectsToDeactivateInVR; // Αντικείμενα που πρέπει να απενεργοποιηθούν στη λειτουργία VR
        public GameObject[] accessBarriers; // Αντικείμενα που πρέπει να ενεργοποιηθούν στη λειτουργία VR

        public GameObject label; // Ετικέτα που εμφανίζεται στη λειτουργία VR
        public GameObject compass; // Πυξίδα που χρησιμοποιείται στη λειτουργία Game Mode

        public Transform player; // Αναφορά στη μετασχηματισμένη θέση του παίκτη
        public ActivationOfGameStarterNpc scriptToDisableInVR; // Αναφορά στο script που πρέπει να απενεργοποιηθεί στη λειτουργία VR
        public ExplosiveMovement scriptToDisableInVR2; // Δεύτερο script προς απενεργοποίηση στη λειτουργία VR
        public RotatingRing[] scriptToDisableInGM; // Πίνακας scripts που πρέπει να απενεργοποιηθούν στη λειτουργία Game Mode

        private void Start()
        {
            // Ανάκτηση της επιλεγμένης λειτουργίας από τις ρυθμίσεις του χρήστη
            string selectedMode = PlayerPrefs.GetString("SelectedMode", "Game Mode");
            
            // Ενεργοποίηση των φραγμών πρόσβασης
            foreach (var obj2 in accessBarriers)
            {
                obj2.SetActive(true);
            }

            // Απενεργοποίηση συγκεκριμένου script
            if (scriptToDisableInVR2 != null)
                scriptToDisableInVR2.enabled = false;

            // Έλεγχος αν η επιλεγμένη λειτουργία είναι "Virtual Reality"
            if (selectedMode == "Virtual Reality")
            {
                label.SetActive(true); // Εμφάνιση της ετικέτας
                compass.SetActive(false); // Απόκρυψη της πυξίδας
                
                // Απενεργοποίηση αντικειμένων για VR
                foreach (var obj in objectsToDeactivateInVR)
                {
                    obj.SetActive(false);
                }

                // Απενεργοποίηση συγκεκριμένου script
                if (scriptToDisableInVR != null)
                    scriptToDisableInVR.enabled = false;

                // Τοποθέτηση του παίκτη στη σωστή θέση για VR
                player.position = new Vector3(757.615662f, 0.999999225f, 547.593079f); // Προσαρμογή στην αρχική θέση VR
            }
            else if (selectedMode == "Game Mode")
            {
                // Απενεργοποίηση των scripts για Game Mode
                foreach (var script in scriptToDisableInGM)
                {
                    if (script != null)
                        script.enabled = false;
                }

                label.SetActive(false); // Απόκρυψη της ετικέτας

                // Τοποθέτηση του παίκτη στη σωστή θέση για Game Mode
                player.position = new Vector3(640.748047f, 0.982560635f, 294.887085f); // Προσαρμογή στην αρχική θέση του Game Mode

                // Περιστροφή του παίκτη ώστε να βλέπει προς συγκεκριμένη κατεύθυνση
                player.rotation = Quaternion.Euler(0f, 145f, 0f); // Περιστροφή κατά 145 μοίρες στον άξονα Y
            }
        }
    }
}
