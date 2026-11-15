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
        if(chanceNumber >= 6)
        {
            //Get random item from pickuplist and spawn it
        
            GameObject.Instantiate(pickupArray[Random.Range(0, pickupArray.Length)], this.transform.parent);
        }
        else
        {
            return;
        }
    }
}
