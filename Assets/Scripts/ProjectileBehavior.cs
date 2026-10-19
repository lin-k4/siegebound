using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 20f;
    public float rotationSpeed = 0f;
    public float lifeTime = 5f;
    public int damageValue = 10;
    public GameObject bulletHitVFX;
    public AudioClip bulletHitSound;
    public int aoeRadius = 0;

    Transform target;
    Rigidbody rb;

    Vector3 lastKnownDirection = Vector3.forward;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
        
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            lastKnownDirection = (target.position - transform.position).normalized;
        }

        Vector3 desiredVelocity = lastKnownDirection * speed;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, rotationSpeed * Time.fixedDeltaTime);
        if (rb.linearVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    public void SetTarget(Transform currentTarget)
    {
        target = currentTarget;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name + " | tag: " + collision.gameObject.tag);
        if (!collision.gameObject.CompareTag("Enemy") &&
            !collision.gameObject.CompareTag("FlyingEnemy")) return;

        EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageValue);
        }

        if(aoeRadius != 0) 
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("FlyingEnemy"))
                {
                    EnemyBehavior aoeEnemy = collider.gameObject.GetComponent<EnemyBehavior>();
                    if (aoeEnemy )
                    {
                        aoeEnemy.TakeDamage(damageValue);
                    }
                }
            }
        }

        if (bulletHitVFX && collision.contacts.Length > 0)
        {
            Instantiate(bulletHitVFX, collision.contacts[0].point, Quaternion.identity);
        }
        if (bulletHitSound)
        {
            AudioSource.PlayClipAtPoint(bulletHitSound, Camera.main.transform.position, 0.05f);
        }

        Destroy(gameObject);
    }
}
