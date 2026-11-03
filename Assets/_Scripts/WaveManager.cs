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
    public UpgradeMenu upgradeMenu;

    [Header("UI Display Components")]
    public TextMeshProUGUI enemiesRemaining;
    public TextMeshProUGUI waveUi;

    [Header("Wave Configuration Settings")]
    public int currentWave = 1;
    public int enemiesToSpawn = 3;
    private bool isWaveActive = false;

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

        // Ensure we always possess a valid player location update step
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesRemaining != null)
            enemiesRemaining.text = ("Enemies: " + activeEnemies.Length.ToString());

        if (waveUi != null)
            waveUi.text = ("Wave: " + currentWave);

        if (activeEnemies.Length == 0)
        {
            isWaveActive = false;
            Debug.Log("Wave cleared successfully");
            OnWaveCleared();
        }
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(1f);

        // Fallback finder safety step
        if (playerTransform == null && playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // Spawning loop rewritten to calculate random points out on the field
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = CalculateRandomRadiusPosition();

            // Instantiate the enemy unit facing toward the center player coordinate location
            Vector3 lookDirection = (playerTransform != null) ? (playerTransform.position - spawnPosition).normalized : Vector3.forward;
            lookDirection.y = 0f; // Lock horizontal horizon vectors
            Quaternion spawnRotation = (lookDirection.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(lookDirection) : Quaternion.identity;

            Instantiate(enemyPrefab, spawnPosition, spawnRotation);

            // Stagger spawn frames to prevent frame stuttering spikes
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Spawning loop finished successfully!");
        isWaveActive = true;
    }

    private Vector3 CalculateRandomRadiusPosition()
    {
        // Safe baseline if player is destroyed or missing
        if (playerTransform == null)
        {
            return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        }

        // 1. Choose a completely random direction vector heading (0 to 360 degrees in radians)
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // 2. Select a random distance stepping factor boundary space range limits
        float randomDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        // 3. Shift offset vectors outward matching the player's core anchor footprint center point
        float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle) * randomDistance;

        // 4. Force surface ground terrain Y height matching plane boundaries
        float spawnY = playerTransform.position.y;

        return new Vector3(spawnX, spawnY, spawnZ);
    }

    private void OnWaveCleared()
    {
        if (isWaveActive) return;

        // Open upgradeMenu
        if (upgradeMenu != null)
        {
            upgradeMenu.ToggleMenu();
        }

        currentWave++;
        enemiesToSpawn += 2;
    }

    public void ResumeNextWave()
    {
        StartCoroutine(StartNextWave());
    }

    // Draws the invisible boundaries inside the editor viewport scene tab grid layout view
    private void OnDrawGizmosSelected()
    {
        if (playerObj == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerObj.transform.position, minimumSpawnDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerObj.transform.position, maximumSpawnDistance);
    }
}