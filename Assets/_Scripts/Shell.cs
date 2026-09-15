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

        // Destroy this shell immediately on impact
        Destroy(gameObject);
    }
}
