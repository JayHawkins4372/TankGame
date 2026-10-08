using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HealthUi : MonoBehaviour
{
    //Ui Components
    public Slider healthSlider;

    //player
    public Transform playerTransform;
    private Vector3 offset = new Vector3(0, 3f, 0);

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position= playerTransform.position + offset;
        }

        //get maxHealth from visual script
        healthSlider.maxValue = Unity.VisualScripting.Variables.Application.Get<float>("maxHealth");

        healthSlider.value = Unity.VisualScripting.Variables.Application.Get<float>("currentHealth");
        Debug.Log($"[HealthUi] Current player health: {Unity.VisualScripting.Variables.Application.Get<float>("currentHealth")}");

    }
}
