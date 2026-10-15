using UnityEngine;

public class JoystickStickVisual : MonoBehaviour
{
    // Drag move handle here
    [SerializeField] private RectTransform knob;
    private RectTransform stickRect;

    void Start()
    {
        stickRect = GetComponent<RectTransform>();

        stickRect.anchorMin = new Vector2(0.5f, 0.5f);
        stickRect.anchorMax = new Vector2(0.5f, 0.5f);
        stickRect.pivot = new Vector2(0.5f, 0f);
    }

    void Update()
    {
        if (knob != null)
        {
            // Adjusts the stick's rotation and height to always point exactly at the knob
            Vector2 direction = knob.anchoredPosition;
            float distance = direction.magnitude;

            stickRect.anchoredPosition = Vector2.zero;

            stickRect.sizeDelta = new Vector2(stickRect.sizeDelta.x, distance);

            if (distance > 0.001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                stickRect.localRotation = Quaternion.Euler(0, 0, angle - 90f);
            }
        }
    }
}
