using System.Collections;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public GameObject shellToSpawn;
    public float spawnInterval = 2f;
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
            // Combine spawner's rotation, define desired rotation angle (X, Y, Z)
            Quaternion customRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);

            //Define desired position offset (X, Y, Z)
            Vector3 positionOffset = new Vector3(0f, 0f, 1.5f);

            // 3. Combine the spawner's position with offset
            Vector3 customPosition = transform.position + transform.TransformDirection(positionOffset);

            Instantiate(shellToSpawn, customPosition, customRotation);
        }
    }
}
