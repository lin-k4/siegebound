using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    LevelManager lm;
    Animator animator;
    public float speed = 5f;
    public int startIndex = 0;
    private int currentNodeIndex = 0;
    private GameObject[] nodes = new GameObject[0];

    void Start()
    {
        lm = FindAnyObjectByType<LevelManager>();
        animator = GetComponent<Animator>();
        var nodeList = FindObjectsByType<NodeBehavior>(FindObjectsSortMode.None);
        var sorted = nodeList.OrderBy(n => n.nodeID).ToArray();
        nodes = sorted.Select(n => n.gameObject).ToArray();

        if (nodes.Length > 0)
            currentNodeIndex = Mathf.Clamp(startIndex, 0, nodes.Length - 1);
    }

    // paths twoards the next node in the list
    void Update()
    {
        if (!lm.isPlaying)
        {
            if (animator != null) animator.enabled = false;
            return;
        }

        if (animator != null && !animator.enabled) animator.enabled = true;

        if (currentNodeIndex >= nodes.Length) return;

        GameObject target = nodes[currentNodeIndex];
        float distance = Vector3.Distance(transform.position, target.transform.position);

        const float arriveThreshold = 0.1f;
        if (distance <= arriveThreshold)
        {
            currentNodeIndex++;
            if (currentNodeIndex >= nodes.Length) { enabled = false; return; }
            target = nodes[currentNodeIndex];
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        Vector3 direction = target.transform.position - transform.position;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
