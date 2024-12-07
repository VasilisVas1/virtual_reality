using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FantasyKingdom
{
    public class AgentPatrol : MonoBehaviour
    {
        NavMeshAgent agent;
        //Animator animator;
        [SerializeField] private Transform[] points;
        [SerializeField] private Transform player;
        int pointIndex = 0; // Start at the first point
        bool isPlayerInRange = false;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            //animator = GetComponent<Animator>();

            GoToPoint(); // Start patrolling
        }

        // Update is called once per frame
        void Update()
        {
            if (isPlayerInRange)
            {
                // NPC looks at the player when in range
                LookAtPlayer();
            }
            else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // When the NPC reaches its target, go to the next point
                GoToNextPoint();
            }
        }

        private void GoToPoint()
        {
            if (points.Length == 0) return; // Safety check

            // Set the agent's destination to the current patrol point
            agent.SetDestination(points[pointIndex].position);
        }

        private void GoToNextPoint()
        {
            // Increment the patrol point index (loop back to start if necessary)
            pointIndex = (pointIndex + 1) % points.Length;
            GoToPoint();
        }

        private void LookAtPlayer()
        {
            // Rotate smoothly to face the player
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
                //animator.SetBool("Walking", false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                agent.isStopped = false; // Resume the agent's movement
                // Resume moving to the current destination
                agent.SetDestination(points[pointIndex].position);
                //animator.SetBool("Walking", true);
            }
        }
    }
}
