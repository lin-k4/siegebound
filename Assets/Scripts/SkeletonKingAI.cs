using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SkeletonKingAi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    LevelManager lm;
    NavMeshAgent agent;
    Animator animator;
    Transform target;
    public float movementSpeed = 10f;
    public GameObject minionPrefab;
    public float minionSpawnInterval = 3f;
    void Start()
    {
        lm = FindAnyObjectByType<LevelManager>();
        target = lm.lastNode.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = movementSpeed;
        agent.SetDestination(target.position);
        if(!minionPrefab)
            return;
        StartCoroutine(SpawnSkeleton());
    }
    IEnumerator SpawnSkeleton()
    {
        while(true)
        {
            SpawnMinion();
            yield return new WaitForSeconds(minionSpawnInterval);
        }
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

    void SpawnMinion()
    {
        FindAnyObjectByType<EnemySpawner>().OnEnemySpawned();
        Vector3 spawnPos = transform.position + transform.forward * 1.0f;
        Instantiate(minionPrefab, spawnPos, transform.rotation);
    }
}
