using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval;
}

[System.Serializable]
public class Wave
{
    public List<EnemySpawnEntry> enemies;
    public float timeBetweenGroups;
}

public class EnemySpawner : MonoBehaviour
{
    LevelManager lm;
    public List<Wave> waves;
    int currentWave = 0;
    int enemiesAlive = 0;
    public TMP_Text waveClearText;

    void Start()
    {
        lm = FindAnyObjectByType<LevelManager>();
        UpdateWaveUi();
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWave < waves.Count && lm.isPlaying)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWave]));

            // wait until all enemies are dead
            yield return new WaitUntil(() => enemiesAlive <= 0);

            if (waveClearText != null)
                StartCoroutine(ShowWaveClear());
            currentWave++;
            UpdateWaveUi();
        }

        if (lm.isPlaying)
            lm.LevelWon();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        List<Coroutine> groups = new List<Coroutine>();

        // spawns each enemy type by group
        foreach (EnemySpawnEntry entry in wave.enemies)
        {
            groups.Add(StartCoroutine(SpawnGroup(entry)));
        }

        // wait for all groups to finish
        foreach (Coroutine c in groups)
        {
            yield return c;
        }
    }

    IEnumerator SpawnGroup(EnemySpawnEntry entry)
    {
        for (int i = 0; i < entry.count; i++)
        {
            if (!lm.isPlaying) yield break;
            Instantiate(entry.enemyPrefab, transform.position, transform.rotation);
            enemiesAlive++;
            yield return new WaitForSeconds(entry.spawnInterval);
        }
    }

    IEnumerator ShowWaveClear()
    {
        // show wave clear text
        waveClearText.text = "Wave " + (currentWave + 1) + " Complete!";
        waveClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        waveClearText.gameObject.SetActive(false);
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
    }

    void OnDestroy()
    {
        Debug.Log("Spawner destroyed by: " + gameObject.name);
    }

    public void UpdateWaveUi()
    {
        // updates current wave ui
        lm.waveSlider.value = ((float)currentWave) / waves.Count;
        lm.waveText.text = "Wave: " + Mathf.Clamp((currentWave + 1), 1 , waves.Count) + "/" + waves.Count;
    }

    public void OnEnemySpawned()
    {
        enemiesAlive++;
    }
}
