/*
 * Author [Lopez-Sotelo, Jorge]
 * Date Created [09/01/2029]
 * Date Modified [09/30/2026 16:04]
 */

using Unity.VisualScripting;
using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    [Header("Flight")]
    public float speed = 7f;
    public float lifetime = 20f;

    [Header("Damage")]
    public float damage = 10f;
    public string scriptGraphDamageEvent = "TakeDamage";


    [SerializeField] GameObject damagePopupPrefab; // for Ui

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); //Direction  of the shell
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (FindTagged(collision.transform, "Enemy") !=null) return;

        if (collision.gameObject.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            Transform player = FindTagged(collision.transform, "Player");

            if (player != null) HurtPlayer(player);
        }

        Destroy(gameObject);
    }

    static Transform FindTagged(Transform t, string tag)
    {
        for (; t != null; t = t.parent)
            if (t.CompareTag(tag)) return t;
        return null;
    }

    void HurtPlayer(Transform player)
    {
        ScriptMachine[] machines = player.GetComponentsInChildren<ScriptMachine>();
        if (machines.Length == 0)
        {
            return;
        }

        foreach (ScriptMachine m in machines)
            CustomEvent.Trigger(m.gameObject, scriptGraphDamageEvent, damage);
    }
}
