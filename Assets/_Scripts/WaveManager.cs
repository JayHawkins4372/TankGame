//Author: Wade Lawler
//Last modified: 9/16/2026
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public UpgradeMenu upgradeMenu;
    public Transform[] spawnPoints;

    public int currentWave = 1;
    public int enemiesToSpawn = 3;
    private bool isWaveActive = false;

    //The minimum safe distance to spawn from the player
    public float minimumSpawnDistance = 15f;
    //Temporary hold for player position
    private Transform playerTransform;
    //store player here
    public GameObject playerObj;

    void Start()
    {
        // Find and store the player
        //GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        //game started, start first wave
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (!isWaveActive) return;

        //update player position

        playerTransform = playerObj.transform;

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (activeEnemies.Length == 0)
        {
            Debug.Log("Wave cleared successfully");
            isWaveActive = false;
            OnWaveCleared();
        }
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(1f);

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Points array is empty");
            yield break;
        }

        // Ensure we have the player referenced
      //  if (playerTransform == null)
      //  {
      //      GameObject playerObj = GameObject.FindWithTag("Player");
      //      if (playerObj != null) playerTransform = playerObj.transform;
      //  }


        List<int> availableIndices = new List<int>();

        //spawning enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (availableIndices.Count == 0)
            {
                for (int j = 0; j < spawnPoints.Length; j++)
                {
                    availableIndices.Add(j);
                }
            }

            int chosenSpawnPointIndex = -1;
            int poolIndexToUse = -1;

            // Look through available pool of spawn points to find a point the player cannot see
            for (int k = 0; k < availableIndices.Count; k++)
            {
                int testIndex = availableIndices[k];
                Transform testPoint = spawnPoints[testIndex];

                if (testPoint == null) continue;

                // If the chosen spawn point is far enough from player, choose it
                if (IsPointSafe(testPoint.position))
                {
                    chosenSpawnPointIndex = testIndex;
                    poolIndexToUse = k;
                    break;
                }
            }

            //  Fallback: 
            // If the remaining pool options are all dangerous, check all spawn points in the game
            // to find a safe one, even if it means reusing a spawnpoint from previous frame.
            if (chosenSpawnPointIndex == -1)
            {
                Debug.LogWarning("Pool ran out of safe points. Scanning all scene points...");
                for (int j = 0; j < spawnPoints.Length; j++)
                {
                    if (spawnPoints[j] != null && IsPointSafe(spawnPoints[j].position))
                    {
                        chosenSpawnPointIndex = j;
                        // not using poolIndexToUse here because it's pulling from the overall list, not the current pool
                        poolIndexToUse = -1;
                        break;
                    }
                }
            }

            // absolute worst-case scenario safety check (player is touching every single point on map)
            if (chosenSpawnPointIndex == -1)
            {
                poolIndexToUse = 0;
                chosenSpawnPointIndex = availableIndices[poolIndexToUse];
                Debug.LogWarning("CRITICAL: All spawn points too close to player. Forcing spawn at fallback point.");
            }

            // Only remove from the pool if it actually picked a spawnpoint out of it
            if (poolIndexToUse != -1)
            {
                availableIndices.RemoveAt(poolIndexToUse);
            }

            Transform selectedPoint = spawnPoints[chosenSpawnPointIndex];
            if (selectedPoint == null) continue;

            Instantiate(enemyPrefab, selectedPoint.position, selectedPoint.rotation);

            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Spawning loop finished successfully!");
        isWaveActive = true;
    }

    // This function checks if a spawnpoint is top close to the player or not.
    //returns true if distance is greater than minimumSpawnDistance
    private bool IsPointSafe(Vector3 spawnPosition)
    {
        // If the player doesn't exist in the scene any point is safe
        if (playerTransform == null) return true;

        // Calculate distance between the player and the spawn point
        float distance = Vector3.Distance(playerTransform.position, spawnPosition);

        // It is only safe if the distance is greater than minimumSpawnDistance var
        return distance >= minimumSpawnDistance;
    }

    private void OnWaveCleared()
    {
        //open upgradeMenu
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
}
