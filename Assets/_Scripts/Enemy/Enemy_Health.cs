//Author: Wade lawler
//Last Modified: 10/13/26
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject[] pickupArray;
    Vector3 spawnPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            TrySpawnPickup();
            Die();
        }
    }

    private void Die()
    {
        WaveManager manager = FindAnyObjectByType<WaveManager>();
        if (manager != null)
        {
            manager.EnemyKilled(); // Subtracts 1 from the live counter instantly
        }
        Destroy(gameObject);
    }
    private void TrySpawnPickup()
    {
        int chanceNumber;
        chanceNumber = Random.Range(1, 10);
        if(chanceNumber == 10)
        {
            //Get random item from pickuplist and spawn it
            Vector3 spawnPoint = transform.position + (Vector3.up * 1);
            Instantiate(pickupArray[Random.Range(0, pickupArray.Length)], spawnPoint, transform.rotation);
        }
        else
        {
            return;
        }
    }
}
