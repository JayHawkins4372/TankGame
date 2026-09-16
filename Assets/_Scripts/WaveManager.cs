//Author: Wade Lawler
//Last modified: 9/16/2026
using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject baseEnemy;
    public UpgradeMenu upgradeMenu;
    public Transform[] spawnPoints;

    private int currentWave = 1;
// Starting enemy amount for Wave 1
    private int enemiesToSpawn = 3;
    private bool isWaveActive = false;

    void Update()
    {
        // nothing if wave isn't active
        if (!isWaveActive) return;

        // Count enemies in scene
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies left, end wave
        if (activeEnemies.Length == 0)
        {
            isWaveActive = false;
            OnWaveCleared();
        }
    }

    private IEnumerator StartNextWave()
    {
        // Brief delay before enemies start appearing
        yield return new WaitForSeconds(1f); 

        // Spawn loop based on current wave settings
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Picks random spawn point from spawnPoints array
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(baseEnemy, randomPoint.position, randomPoint.rotation);

        // Half-second stagger delay between each tank spawn
            yield return new WaitForSeconds(0.5f); 
        }

        isWaveActive = true;
    }

    private void OnWaveCleared()
    {
        // 1. Call your existing public menu activation logic!
        if (upgradeMenu != null)
        {
            upgradeMenu.ToggleMenu();
        }

        // 2. Scale difficulty modifiers for when they return from the menu
        currentWave++;
        enemiesToSpawn += 2; // Adds 2 extra tanks to the spawn queue every single wave
    }

    // Call this specific method from your UI "Close Menu" or "Resume" button
    public void ResumeNextWave()
    {
        StartCoroutine(StartNextWave());
    }
}
