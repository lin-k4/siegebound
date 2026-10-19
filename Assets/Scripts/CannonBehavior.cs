using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    public AudioClip shootSFX;
    AudioSource audioSource;
    LevelManager lm;
    public enum TowerState
    {
        Patrol, Attack
    }
    public TowerState currentState = TowerState.Patrol;
    public GameObject rangeVisualizer;

    [Header("Cost Settings")]
    public int totalCost = 350;
    public GameObject upgradePrefab = null;
    public int upgradePrice;

    [Header("Patrol Settings")]
    public float rotationSpeed = 10f;

    [Header("Attack Settings")]
    public float detectionRange = 10f;

    Transform target;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int fireRate = 2;
    float fireCooldown = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lm = FindAnyObjectByType<LevelManager>();
        if (rangeVisualizer != null)
        {
            rangeVisualizer.transform.localScale = new Vector3(detectionRange * 2f, detectionRange * 2f, detectionRange * 2f);
            rangeVisualizer.SetActive(false);
        }
    }

    void Update()
    {
        if (!lm.isPlaying)
        {
            return;
        }
        switch (currentState)
        {
            case TowerState.Patrol:
                Patrol();
                break;
            case TowerState.Attack:
                Attack();
                break;
        }
    }

    //searches for an enemy in range
    void Patrol()
    {
        LookForEnemies();
    }

    //if an enemy is in range, attack
    void Attack()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) > detectionRange)
        {
            target = null;
            currentState = TowerState.Patrol;
            return;
        }
        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        if (fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }

        fireCooldown -= Time.deltaTime;
    }

    //see if any enemies are in range, finds nearest enemy
    void LookForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = collider.transform;
                }
            }
        }

        if (nearestEnemy)
        {
            target = nearestEnemy;
            currentState = TowerState.Attack;
        }
    }

    //instantiates projectile
    void Shoot()
    {
        if (shootSFX != null) audioSource.PlayOneShot(shootSFX);
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        ProjectileBehavior bulletBehavior = bullet.GetComponent<ProjectileBehavior>();
        if (bulletBehavior)
        {
            bulletBehavior.SetTarget(target);
        }
    }

    //enables range visualizer
    public void IsSelected()
    {
        rangeVisualizer.SetActive(true);
    }

    //disables range visualizer
    public void IsDeselected()
    {
        rangeVisualizer.SetActive(false);
    }

    //upgrades tower if player has enough money, otherwise does nothing
    public GameObject AttemptUpgrade()
    {
        if (upgradePrefab != null)
        {
            if(lm.money < upgradePrice)
            {
                return gameObject;
            }
            lm.money -= upgradePrice;
            lm.UpdateMoneyText();
            GameObject upgradedTower = Instantiate(upgradePrefab, transform.position, transform.rotation);
            CannonBehavior newTowerScript = upgradedTower.GetComponent<CannonBehavior>();

            if (newTowerScript != null)
            {
                newTowerScript.totalCost = this.totalCost + this.upgradePrice;
            }

            Destroy(gameObject);
            return upgradedTower;
        }
        return gameObject;
    }

    //gets tower type for UI display
    public string GetTowerTypeText()
    {
        if (gameObject.name.Contains("(Clone)"))
        {
            return gameObject.name.Substring(0, gameObject.name.IndexOf("(Clone)"));
        }
        return gameObject.name;
    }

    //gets stats for UI display
    public string GetStats()
    {
        int dmg = projectilePrefab.GetComponent<ProjectileBehavior>() ? projectilePrefab.GetComponent<ProjectileBehavior>().damageValue : 0;
        return "Damage: " + dmg + "\n" +
               "Fire Rate: " + fireRate + " shots/sec\n" +
               "Range: " + detectionRange + " units";
    }

    //gets upgrade stats for UI display
    public string GetUpgradeStats()
    {
        if (upgradePrefab != null)
        {
            CannonBehavior upgradeTower = upgradePrefab.GetComponent<CannonBehavior>();
            int currentDmg = projectilePrefab.GetComponent<ProjectileBehavior>() ? projectilePrefab.GetComponent<ProjectileBehavior>().damageValue : 0;
            int upgradeDmg = upgradeTower.projectilePrefab.GetComponent<ProjectileBehavior>() ? upgradeTower.projectilePrefab.GetComponent<ProjectileBehavior>().damageValue : 0;

            return "Damage: \n" + currentDmg + " -> " + upgradeDmg + "\n" +
                   "Fire Rate: \n" + fireRate + " -> " + upgradeTower.fireRate + " shots/sec\n" +
                   "Range: \n" + detectionRange + " -> " + upgradeTower.detectionRange + " units";
        }
        return "Max Level Reached";
    }

    //gets upgrade price for UI display
    public string GetUpgradeCost()
    {
        return upgradePrefab != null ? "$" + upgradePrice : "N/A";
    }

    //gets sell price for UI display
    public string GetSellValue()
    {
        if (lm == null) return "Sell Value: $0";
        return "Sell Value: $" + Mathf.RoundToInt(totalCost * lm.sellValue);
    }
}