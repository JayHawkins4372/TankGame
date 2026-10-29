using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor.VisualStudioIntegration;

public class VignetteController : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        if(!volume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette NOT found!");
        }
    }
    // Update is called once per frame
    public void AdjustVignette(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.value = intensity;
        }
    }
}
