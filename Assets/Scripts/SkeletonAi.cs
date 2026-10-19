using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SkeletonAi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    LevelManager lm;
    NavMeshAgent agent;
    Animator animator;
    Transform target;
    public float movementSpeed = 10f;
    void Start()
    {
        lm = FindAnyObjectByType<LevelManager>();
        target = lm.lastNode.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = movementSpeed;
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!lm.isPlaying)
        {
            if (!agent.isStopped) agent.isStopped = true;
            if (animator != null) animator.enabled = false;
            return;
        }
        if (agent.isStopped) agent.isStopped = false;
        if (animator != null && !animator.enabled) animator.enabled = true;
        
    }
}
