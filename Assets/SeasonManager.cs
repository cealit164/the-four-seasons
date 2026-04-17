using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    [Header("Configurations")]
    public SeasonConfiguration currentSeason;
    public float transitionSpeed = 2.0f;

    [Header("Scene References")]
    public Light sunLight;
    public Material fogMaterial;
    public MeshRenderer[] cityTrees;

    // Internal tracking for smooth Lerping
    private float transitionTimer;

    void Update()
    {
        ApplySeason(currentSeason);
    }

    public void ApplySeason(SeasonConfiguration config)
    {
        if (config == null) return;

        // 1. Update Skybox
        RenderSettings.skybox = config.skyboxMaterial;

        // 2. Update Directional Light
        sunLight.color = config.sunColor;
        sunLight.intensity = config.sunIntensity;
        sunLight.transform.rotation = Quaternion.Euler(config.middayRotation);

        // 3. Update Fog Material (Based on your uploaded image)
        // Note: Check your Shader Graph 'Reference' names if these don't react!
        fogMaterial.SetColor("_BaseColor", config.fogBaseColor);
        fogMaterial.SetColor("_EmissionColor", config.fogEmissionColor);
        fogMaterial.SetFloat("_FadeDistanceFar", config.fogFarDistance);
        fogMaterial.SetFloat("_DistortionStrength", config.fogDistortionStrength);

        // 4. Update Trees/Models
        foreach (var tree in cityTrees)
        {
            if (tree != null) tree.material = config.treeMaterial;
        }

        // 5. Global Cloud Parameters
        Shader.SetGlobalFloat("_GlobalCloudDensity", config.cloudDensity);
        Shader.SetGlobalColor("_GlobalCloudColor", config.cloudColor);

        //Debug.Log($"Season Switched to: {config.name}");
    }
}