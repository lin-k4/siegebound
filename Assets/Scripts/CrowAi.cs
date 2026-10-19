using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CrowAi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    LevelManager lm;
    NavMeshAgent agent;
    Animator animator;
    void Start()
    {
        lm = FindAnyObjectByType<LevelManager>();
        animator = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
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
