using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class AgentAngry : MonoBehaviour
    {
        Animator animator;
        [SerializeField] private Transform player;
        [SerializeField] private float lookSpeed = 5f;
        private Quaternion initialRotation;

        private bool isLookingAtPlayer = false;
        private bool isReturningToInitialRotation = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            initialRotation = transform.rotation;
            animator.Play("arguing_animation", 0, 0f); // Play the arguing animation immediately on start
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isLookingAtPlayer)
            {
                // Stop any ongoing rotation coroutine
                if (isReturningToInitialRotation)
                {
                    StopAllCoroutines(); // Stop the current rotation coroutine if it's running
                    isReturningToInitialRotation = false;
                }

                isLookingAtPlayer = true;

                // Transition to the "looking" animation immediately
                animator.Play("looking", 0, 0f);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isLookingAtPlayer)
            {
                isLookingAtPlayer = false;

                // Transition to the "arguing_animation" immediately
                animator.Play("arguing_animation", 0, 0f);

                // Start the coroutine to return to initial rotation
                if (!isReturningToInitialRotation)
                {
                    isReturningToInitialRotation = true;
                    StartCoroutine(ReturnToInitialRotation());
                }
            }
        }

        void Update()
        {
            if (isLookingAtPlayer)
            {
                LookAtPlayer();
            }
        }

        private void LookAtPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
        }

        private System.Collections.IEnumerator ReturnToInitialRotation()
        {
            while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * lookSpeed);
                yield return null;
            }
            transform.rotation = initialRotation;
            isReturningToInitialRotation = false;
        }
    }
}
