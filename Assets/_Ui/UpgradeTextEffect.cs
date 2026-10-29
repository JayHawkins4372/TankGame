using UnityEngine;
using TMPro;

public class UpgradeTextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float floatSpeed = 1.8f;
    [SerializeField] private float lifetime = 1.2f; // Slightly longer so player can read it

    private Transform playerTransform;
    private Vector3 positionOffset;
    private Color originalColor;
    private float timer;

    void Start()
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null) originalColor = textMesh.color;

        Destroy(gameObject, lifetime);
    }

    // Call this from your Upgrade Menu code to set custom text and find the player
    public void SetupUpgradePopup(string textToShow, Transform player)
    {
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null) textMesh.text = textToShow;

        playerTransform = player;

        // Starts 3 units above the player tank center so it clears the turret
        positionOffset = new Vector3(0f, 3f, 0f);
    }

    void LateUpdate()
    {
        // Smoothly track the player tank's position while floating upward
        if (playerTransform != null)
        {
            positionOffset += Vector3.up * floatSpeed * Time.deltaTime;
            transform.position = playerTransform.position + positionOffset;
        }
        else
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        }

        // Always face the main camera
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        // Fade away animation
        if (textMesh != null)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
    }
}