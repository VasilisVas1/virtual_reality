using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class AgentPatrol : MonoBehaviour
    {
        NavMeshAgent agent;
        Animator animator; // Reference to the Animator
        [SerializeField] private Transform[] points;
        [SerializeField] private Transform player;
        int pointIndex = 0; // Start at the first point
        bool isPlayerInRange = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            GoToPoint(); // Start patrolling
        }

        void Update()
        {
            if (isPlayerInRange)
            {
                LookAtPlayer();
                animator.SetBool("IsWalking", false); // Switch to idle animation
            }
            else
            {
                animator.SetBool("IsWalking", !agent.isStopped); // Walk if the agent is moving

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
                {
                    GoToNextPoint();
                }
            }
        }

        private void GoToPoint()
        {
            if (points.Length == 0) return; // Safety check

            agent.SetDestination(points[pointIndex].position);
        }

        private void GoToNextPoint()
        {
            pointIndex = (pointIndex + 1) % points.Length;
            GoToPoint();
        }

        private void LookAtPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Keep rotation on the horizontal plane
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                agent.isStopped = true; // Pause the agent's movement
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                agent.isStopped = false; // Resume the agent's movement
                animator.CrossFade("Walking", 0f); // Immediately transition to walking animation
                GoToPoint(); // Resume moving to the current destination
            }
        }
    }
}

