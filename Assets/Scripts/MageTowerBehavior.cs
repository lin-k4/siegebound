using System.Collections.Generic;
using UnityEngine;

public class MageTowerBehavior : MonoBehaviour
{
    public AudioClip shootSFX;
    AudioSource audioSource;
    LevelManager lm;

    public enum TowerState
    {
        Patrol, Attack, Die
    }
    public TowerState currentState = TowerState.Patrol;
    public GameObject rangeVisualizer;
    public int totalCost = 500;
    public int upgradePrice;
    public LevelManager levelManager;
    [Header("Patrol Settings")]
    public float rotationSpeed = 30f;
    [Header("Attack Settings")]
    public float detectionRange = 10f;
    Transform target;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int fireRate = 2;
    float fireCooldown = 0f;

    public GameObject upgradePrefab = null;

    void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        lm = FindAnyObjectByType<LevelManager>();
        if (rangeVisualizer != null)
        {
            rangeVisualizer.transform.localScale = new Vector3(detectionRange * 2f, detectionRange * 2f, detectionRange * 2f);
            if (levelManager == null || levelManager.selectedPlacedTower != gameObject)
            {
                rangeVisualizer.SetActive(false);
            }
            else
            {
                rangeVisualizer.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(!lm.isPlaying)
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
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        firePoint.transform.rotation = Quaternion.Slerp(firePoint.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

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
            if (collider.CompareTag("Enemy") || collider.CompareTag("FlyingEnemy"))
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
            if (lm.money < upgradePrice)
            {
                return gameObject;
            }
            lm.money -= upgradePrice;
            lm.UpdateMoneyText();
            GameObject upgradedTower = Instantiate(upgradePrefab, transform.position, transform.rotation);
            MageTowerBehavior newTowerScript = upgradedTower.GetComponent<MageTowerBehavior>();

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
        return gameObject.name.Substring(0, gameObject.name.IndexOf("(Clone)"));
    }

    //gets stats for UI display
    public string GetStats()
    {
        return "Damage: " + projectilePrefab.GetComponent<ProjectileBehavior>().damageValue + "\n" +
               "Fire Rate: " + fireRate + " shots/sec\n" +
               "Range: " + detectionRange + " units";
    }

    //gets upgrade stats for UI display
    public string GetUpgradeStats()
    {
        if (upgradePrefab != null)
        {
            ProjectileBehavior upgradeProjectile = upgradePrefab.GetComponent<MageTowerBehavior>().projectilePrefab.GetComponent<ProjectileBehavior>();
            MageTowerBehavior upgradeTower = upgradePrefab.GetComponent<MageTowerBehavior>();
            return "Damage: \n" + gameObject.GetComponent<MageTowerBehavior>().projectilePrefab.GetComponent<ProjectileBehavior>().damageValue + " -> " + upgradeProjectile.damageValue + "\n" +
                   "Fire Rate: \n" + gameObject.GetComponent<MageTowerBehavior>().fireRate + " -> " + upgradeTower.fireRate + " shots/sec\n" +
                   "Range: \n" + gameObject.GetComponent<MageTowerBehavior>().detectionRange + " -> " + upgradeTower.detectionRange + " units";
        }
        else
        {
            return "No further upgrades available.";
        }
    }

    //gets upgrade price for UI display
    public string GetUpgradeCost()
    {
        if (upgradePrefab != null)
        {
            return "$" + upgradePrice;
        }
        else
        {
            return "N/A";
        }
    }

    //gets sell price for UI display
    public string GetSellValue()
    {
        return "Sell Value: $" + Mathf.RoundToInt(totalCost * levelManager.sellValue);
    }
}
