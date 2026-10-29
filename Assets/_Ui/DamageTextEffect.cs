using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageTextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh; // Drag your DamageText child object here
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float lifetime = 0.8f;

    private Transform targetEnemy;
    private Vector3 positionOffset;
    private Color originalColor;
    private float timer;

    void Start()
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = textMesh.color;

        Destroy(gameObject, lifetime);
    }

    public void SetupPopup(float amount, Transform enemyTransform)
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = Mathf.RoundToInt(amount).ToString();

        targetEnemy = enemyTransform;

        // Define how high above the enemy center the text should start
        // (Adjust the 2.5f up or down if it's still clipping into the tank mesh)
        positionOffset = new Vector3(Random.Range(-0.3f, 0.3f), 2.5f, 0f);
    }

    void LateUpdate()
    {
        // 1. Keep the popup locked to the enemy's moving position, plus our height offset
        if (targetEnemy != null)
        {
            // The offset slowly moves UP over time to create the floating animation
            positionOffset += Vector3.up * floatSpeed * Time.deltaTime;
            transform.position = targetEnemy.position + positionOffset;
        }
        else
        {
            // If the enemy dies/destroys mid-animation, just float in place so it doesn't break
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        }

        // 2. Keep facing the player camera
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        // 3. Fade out
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
