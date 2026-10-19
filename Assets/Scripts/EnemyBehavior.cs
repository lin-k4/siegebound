using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public int totalEnemyHealth = 100;
    public int damageValue = 10;
    public int reward = 50;
    public AudioClip deathSFX;
    public Slider enemyHealthSlider;
    bool isDead = false;
    int currentEnemyHealth;
    AudioSource audioSource;
    LevelManager lm;

    void Start()
    {
        currentEnemyHealth = totalEnemyHealth;
        audioSource = GetComponent<AudioSource>();
        lm = FindAnyObjectByType<LevelManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!lm.isPlaying) return;

        if (other.gameObject.name == lm.lastNode.name)
        {
            lm.TakeDamage(damageValue);
            Die(giveReward: false);
        }
    }

    void Die(bool giveReward = true)
    {
        if (isDead) return;
        isDead = true;

        FindAnyObjectByType<EnemySpawner>().OnEnemyDied();
        if(giveReward)
        {
            lm.money += reward;
            lm.UpdateMoneyText();
        }
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        currentEnemyHealth = Mathf.Max(currentEnemyHealth, 0);

        if (enemyHealthSlider)
        {
            UpdateHealthSlider();
        }
        if (currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthSlider()
    {
        enemyHealthSlider.value = (float)currentEnemyHealth / totalEnemyHealth;
    }
}
