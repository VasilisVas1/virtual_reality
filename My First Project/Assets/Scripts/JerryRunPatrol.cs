using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class JerryRunPatrol : MonoBehaviour
    {
        NavMeshAgent agent;
        Animator animator; // Reference to the Animator
        [SerializeField] private Transform[] points; // Patrol points
        int pointIndex = 0; // Start at the first point

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            // Play the running animation
            animator.SetBool("IsRunning", true);

            GoToPoint(); // Start patrolling
        }

        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
            {
                GoToNextPoint(); // Move to the next point when Jerry reaches the current one
            }
        }

        private void GoToPoint()
        {
            if (points.Length == 0) return; // Safety check

            agent.SetDestination(points[pointIndex].position);
        }

        private void GoToNextPoint()
        {
            pointIndex = (pointIndex + 1) % points.Length; // Loop through the points
            GoToPoint();
        }
    }
}
