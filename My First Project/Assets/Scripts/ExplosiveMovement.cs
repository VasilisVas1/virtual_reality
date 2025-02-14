using UnityEngine;

public class ExplosiveMovement : MonoBehaviour
{
    public GameObject explosivePrefab; // Prefab του εκρηκτικού αντικειμένου
    public float throwForce = 10f; // Δύναμη που εφαρμόζεται για τη ρίψη του εκρηκτικού (αν και εμφανίζεται στα πόδια, μπορείς να το προσαρμόσεις)
    public float explosionForce = 500f; // Δύναμη της έκρηξης
    public float explosionRadius = 5f; // Ακτίνα της έκρηξης
    public int maxUses = 3; // Μέγιστος αριθμός χρήσεων

    private int remainingUses; // Υπολειπόμενες χρήσεις
    private Rigidbody playerRigidbody; // Αναφορά στο Rigidbody του παίκτη
    public GameObject pauseMenuUI; // Αναφορά στο μενού παύσης
    public GameObject settingsPanel; // Αναφορά στο μενού ρυθμίσεων
    public GameObject helpMenu; // Αναφορά στο μενού βοήθειας

    // Μετατόπιση της θέσης εμφάνισης της χειροβομβίδας σε σχέση με τα πόδια του παίκτη (προσαρμόσιμη)
    public Vector3 throwOffset = new Vector3(0, -1, 0);

    void Start()
    {
        remainingUses = maxUses; // Ορισμός των αρχικών χρήσεων στο μέγιστο αριθμό
        playerRigidbody = GetComponent<Rigidbody>(); // Απόκτηση του Rigidbody του παίκτη
    }

    void Update()
    {
        // Αν οποιοδήποτε από τα μενού είναι ενεργό, μην επιτρέπεις τη ρίψη εκρηκτικών
        if (pauseMenuUI.activeSelf || settingsPanel.activeSelf || helpMenu.activeSelf)
        {
            return;
        }

        // Αν ο παίκτης πατήσει το πλήκτρο G και έχει ακόμα διαθέσιμες χρήσεις
        if (Input.GetKeyDown(KeyCode.G) && remainingUses > 0)
        {
            ThrowExplosive(); // Κλήση της μεθόδου ρίψης εκρηκτικού
        }
    }

    void ThrowExplosive()
    {
        remainingUses--; // Μείωση του αριθμού των διαθέσιμων χρήσεων

        // Υπολογισμός της θέσης για να εμφανιστεί η βόμβα λίγο πίσω από τον παίκτη
        Vector3 feetPosition = transform.position + transform.forward * -3 + throwOffset;

        // Δημιουργία του εκρηκτικού αντικειμένου στη νέα θέση
        GameObject explosive = Instantiate(explosivePrefab, feetPosition, Quaternion.identity);

        // Προσθήκη μιας μικρής τυχαίας δύναμης ώθησης για να προσομοιωθεί το "πέταγμα" (προαιρετικό)
        Rigidbody explosiveRb = explosive.GetComponent<Rigidbody>();
        if (explosiveRb != null)
        {
            explosiveRb.AddForce(Vector3.up * throwForce, ForceMode.Impulse); // Προσθήκη μικρής ανοδικής δύναμης
        }

        // Άμεση προσομοίωση της έκρηξης (χωρίς καθυστέρηση)
        Explode(explosive);
    }

    public AudioClip explosionSound; // Ήχος της έκρηξης

    void Explode(GameObject explosive)
    {
        // Αναπαραγωγή του ήχου της έκρηξης στη θέση του εκρηκτικού
        AudioSource.PlayClipAtPoint(explosionSound, explosive.transform.position);

        // Εφέ έκρηξης
        Vector3 explosionPosition = explosive.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);

                // Αν το αντικείμενο που επηρεάζεται είναι ο παίκτης, πρόσθεσε δύναμη προς την κατεύθυνση που κοιτάζει
                if (rb == playerRigidbody)
                {
                    Vector3 forwardDirection = playerRigidbody.transform.forward;
                    playerRigidbody.AddForce(forwardDirection * explosionForce, ForceMode.Impulse);
                }
            }
        }

        // Καταστροφή του εκρηκτικού αντικειμένου αμέσως μετά την έκρηξη
        Destroy(explosive);
    }

    void OnGUI()
    {
        // Δημιουργία ενός GUIStyle για να αλλάξουμε το μέγεθος της γραμματοσειράς
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.fontSize = 24; // Ορισμός μεγέθους γραμματοσειράς

        // Υπολογισμός θέσης σε σχέση με την κάτω αριστερή γωνία της οθόνης
        float xPosition = 20f; // Απόσταση από την αριστερή πλευρά
        float yPosition = Screen.height - 50f; // Απόσταση από το κάτω μέρος της οθόνης

        // Εμφάνιση μηνύματος στην οθόνη με τον αριθμό των εκρηκτικών που απομένουν
        GUI.Label(new Rect(xPosition, yPosition, 600, 30), "Explosives Remaining (Press G to use): " + remainingUses, guiStyle);
    }
}
