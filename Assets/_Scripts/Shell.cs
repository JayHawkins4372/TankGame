using UnityEngine;

public class Shell : MonoBehaviour
{

    public float speed = 10f;
    public float lifetime = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Automatically destroy the shell after 'lifetime' seconds to prevent lag
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object forward relative to its own current rotation every frame
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // This handles the despawning when hitting something
    private void OnCollisionEnter(Collision collision)
    {
        //if collide with player, ignore
        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player")){
            return;
        }
        // 1. Check if the object we ran into has the "Enemy" tag or its parent
        else if (collision.gameObject.CompareTag("Enemy") || collision.transform.root.CompareTag("Enemy"))
        {
            // 2. Delete enemy tank (temporary until health is added)
            Destroy(collision.transform.root.gameObject);
            Destroy(collision.gameObject);
        }
        // Destroy this shell immediately on impact
        Destroy(gameObject);
    }
}
