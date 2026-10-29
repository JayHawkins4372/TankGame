//Author: Wade lawler
//Last Modified: 10/29/26
//Last Modified: 10/06/2026 (Jorge Lopez-Sotelo)
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] GameObject explosionPF;  //Drop PF_Explosion on here from VFX
    [SerializeField] GameObject damagePopupPrefab; // for Ui

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
            Enemy_Health enemy = collision.gameObject.GetComponent<Enemy_Health>();

            if (enemy != null)
            {
                enemy.takeDamage(damage);
                Debug.Log("Found and dealt damage to enemy");

                //for damage Ui
                Vector3 spawnPos = enemy.transform.position + Vector3.up * 2.5f;
                GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

                // Pass the damage number to the text script
                DamageTextEffect effectScript = popup.GetComponent<DamageTextEffect>();
                if (effectScript != null)
                {
                    // Pass the damage number AND the enemy transform so it tracks
                    effectScript.SetupPopup(damage, enemy.transform);
                }
            }

            //After dropping the Explosion Prefab inside serealized field, the explotion will play right where the enemy position was.
            if (explosionPF != null)
                Instantiate(explosionPF, collision.transform.root.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }
        else Destroy(gameObject);
        // Destroy this shell immediately on impact
        //Destroy(gameObject);
    }
}
