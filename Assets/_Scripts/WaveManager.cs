//Author: Wade Lawler
//Last modified: 11/02/2026
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Prefabs & Systems")]
    public GameObject enemyPrefab;
    public GameObject lightEnemy;
    public GameObject heavyEnemy;
    public UpgradeMenu upgradeMenu;

    [Header("UI Display Components")]
    public TextMeshProUGUI enemiesRemaining;
    public TextMeshProUGUI waveUi;

    [Header("UI vars")]
    private int totalEnemiesAlive = 0;

    [Header("Wave Configuration Settings")]
    public int currentWave = 1;
    public int enemiesToSpawn = 3;
    private bool isWaveActive = false;
    public float spawnIntervalDelay = 2.5f;

    [Header("Dynamic Radius Bounds")]
    [Tooltip("Minimum distance from player (Keep just off-screen)")]
    public float minimumSpawnDistance = 25f;
    [Tooltip("Maximum distance from player to prevent them spawning too far away")]
    public float maximumSpawnDistance = 35f;

    [Header("Player References")]
    public GameObject playerObj;
    private Transform playerTransform;

    // Internal tracker for scene cleanup routines
    private GameObject[] activeEnemies;

    void Start()
    {
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // Game started, start first wave
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (!isWaveActive) return;

        if (playerObj != null) playerTransform = playerObj.transform;
    }

    private IEnumerator StartNextWave()
    {
        // 1. Instantly update the wave number layout when the wave begins
        isWaveActive = true;
        totalEnemiesAlive = 0;
        UpdateUIDisplay();

        yield return new WaitForSeconds(1f);

        List<GameObject> enemyPool = new List<GameObject>();

        enemyPool.Add(enemyPrefab);
        enemyPool.Add(lightEnemy);
        enemyPool.Add(heavyEnemy);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = CalculateRandomRadiusPosition();

            Vector3 lookDirection = (playerTransform != null) ? (playerTransform.position - spawnPosition).normalized : Vector3.forward;
            lookDirection.y = 0f;
            Quaternion spawnRotation = (lookDirection.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(lookDirection) : Quaternion.identity;

            // 2. Spawn the enemy and immediately add it to our live counter math
            int randomIndex = Random.Range(0, enemyPool.Count);
            GameObject chosenEnemyPrefab = enemyPool[randomIndex];
            Instantiate(chosenEnemyPrefab, spawnPosition, spawnRotation);
            totalEnemiesAlive++;
            UpdateUIDisplay();

            yield return new WaitForSeconds(spawnIntervalDelay);
        }
    }

    // PUBLIC FUNCTION: Call this from the enemy health script right when they blow up!
    public void EnemyKilled()
    {
        totalEnemiesAlive--;
        UpdateUIDisplay();

        // Check win condition instantly here instead of checking every frame in Update
        if (totalEnemiesAlive <= 0)
        {
            isWaveActive = false;
            Debug.Log("Wave cleared successfully");
            OnWaveCleared();
        }
    }

    private void UpdateUIDisplay()
    {
        if (enemiesRemaining != null) enemiesRemaining.text = "Enemies: " + totalEnemiesAlive;
        if (waveUi != null) waveUi.text = "Wave: " + currentWave;
    }

    private Vector3 CalculateRandomRadiusPosition()
    {
        if (playerTransform == null) return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float randomDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle) * randomDistance;
        float spawnY = playerTransform.position.y;

        return new Vector3(spawnX, spawnY, spawnZ);
    }

    private void OnWaveCleared()
    {
        if (upgradeMenu != null) upgradeMenu.ToggleMenu();
        currentWave++;
        enemiesToSpawn += 2;
    }

    public void ResumeNextWave()
    {
        StartCoroutine(StartNextWave());
    }

    private void OnDrawGizmosSelected()
    {
        if (playerObj == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerObj.transform.position, minimumSpawnDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerObj.transform.position, maximumSpawnDistance);
    }
}