//Author: Wade lawler
//Last Modified: 9/23/26
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float Bulletspeed = 10f;
    public float lifetime = 20f;
    public float damage = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object forward relative to its own current rotation every frame
        transform.Translate(Vector3.up * Bulletspeed * Time.deltaTime);
    }

    // This handles the despawning when hitting something
    private void OnCollisionEnter(Collision collision)
    {
        //if collide with player, ignore


        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            return;
        }
        // 1. Check if the object we ran into has the "Enemy" tag or its parent
        else if (collision.gameObject.CompareTag("Enemy") || collision.transform.root.CompareTag("Enemy"))
        {
            return;
        }
        else Destroy(gameObject);
        // Destroy this shell immediately on impact
        //Destroy(gameObject);
    }
}
