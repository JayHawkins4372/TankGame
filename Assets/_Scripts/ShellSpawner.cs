using System.Collections;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    private GameObject shellToSpawn;
    private float spawnInterval = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //start spawning shells in intervals on begin play
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnChild();
        }
    }

        private void SpawnChild()
    {
        if (shellToSpawn != null)
        {
            Instantiate(shellToSpawn, transform.position, transform.rotation);
        }
    }
}
