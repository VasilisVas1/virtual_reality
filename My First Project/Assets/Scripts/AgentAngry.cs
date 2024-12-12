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
        animator.CrossFade("arguing_animation", 0f); 
    }

    void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Player")) 
        {
            animator.CrossFade("looking", 0f); 
        }
        */
        if (other.CompareTag("Player") && !isLookingAtPlayer)
{
    StopAllCoroutines(); // Stop any ongoing rotation coroutine
    isReturningToInitialRotation = false;
    isLookingAtPlayer = true;
    animator.CrossFade("looking", 0f);
}


    }

    void OnTriggerExit(Collider other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            animator.CrossFade("arguing_animation", 0f); 
            StartCoroutine(ReturnToInitialRotation()); 
        }
        */
        if (other.CompareTag("Player") && isLookingAtPlayer)
{
    isLookingAtPlayer = false;
    animator.CrossFade("arguing_animation", 0f);
    StartCoroutine(ReturnToInitialRotation());
}

        
    }

    void Update()
    {
        /*
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("looking"))
        {
            LookAtPlayer(); 
        }
        */
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
        /*
        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * lookSpeed);
            yield return null;
        }
        transform.rotation = initialRotation; 
        */
        isReturningToInitialRotation = true;
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
