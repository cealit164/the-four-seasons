using UnityEngine;
using System.Collections;

public class SeasonManager : MonoBehaviour
{
    [Header("Configurations")]
    public SeasonConfiguration currentSeason;
    public float transitionDuration = 3.0f;

    [Header("Scene References")]
    public Light sunLight;
    public Material fogMaterial;
    public Material cloudMaterial; // Assign the 'cloud' material from your image
    public MeshRenderer[] cityTrees;

    [Header("Structural Groups")]
    public GameObject normalGroup;
    public GameObject rainGroup;
    public GameObject dustGroup;

    private Coroutine transitionCoroutine;

    [Header("Light Groups")]
    public GameObject[] Light;

    //[Header("Animation Groups")]
    //public Animator[] rainAnimation;

    private void Start()
    {
        foreach(GameObject light in Light)
            light.SetActive(false);

        // Apply the initial season immediately at start
        if (currentSeason != null)
            ApplySeasonImmediate(currentSeason);
    }

    // Call this from your UI Button "On Click()"
    public void TriggerTransition(SeasonConfiguration newConfig)
    {
        if (newConfig == null || newConfig == currentSeason) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionRoutine(newConfig));
    }

    private IEnumerator TransitionRoutine(SeasonConfiguration target)
    {
        // 1. Capture Start Values for Interpolation
        Color startSunColor = sunLight.color;
        float startSunIntensity = sunLight.intensity;
        Quaternion startSunRot = sunLight.transform.rotation;

        Color startFogBase = fogMaterial.GetColor("_BaseColor");
        Color startFogEmission = fogMaterial.GetColor("_EmissionColor");
        float startFogFar = fogMaterial.GetFloat("_FadeDistanceFar");

        Color startCloudBase = cloudMaterial.GetColor("_BaseColor");
        Color startCloudEmission = cloudMaterial.GetColor("_EmissionColor");

        //light
        foreach(GameObject light in Light)
            light.SetActive(target.light);

        /*if (target.currentMode == CityMode.Rain)
            foreach (Animator anim in rainAnimation)
                anim.Play(0);*/

        float time = 0;

        // 2. Immediate Structural Toggles (Infrastructure)
        if (normalGroup != null) normalGroup.SetActive(target.currentMode == CityMode.Normal);
        if (rainGroup != null) rainGroup.SetActive(target.currentMode == CityMode.Rain);
        if (dustGroup != null) dustGroup.SetActive(target.currentMode == CityMode.Dust);

        // Update Tree Materials and Skybox immediately
        RenderSettings.skybox = target.skyboxMaterial;
        foreach (var tree in cityTrees) { if (tree != null) tree.material = target.treeMaterial; }

        // 3. Smooth Transition Loop
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float t = time / transitionDuration;

            // Smoothly move the Sun
            sunLight.color = Color.Lerp(startSunColor, target.sunColor, t);
            sunLight.intensity = Mathf.Lerp(startSunIntensity, target.sunIntensity, t);
            sunLight.transform.rotation = Quaternion.Slerp(startSunRot, Quaternion.Euler(target.middayRotation), t);

            // Smoothly morph the Fog
            fogMaterial.SetColor("_BaseColor", Color.Lerp(startFogBase, target.fogBaseColor, t));
            fogMaterial.SetColor("_EmissionColor", Color.Lerp(startFogEmission, target.fogEmissionColor, t));
            fogMaterial.SetFloat("_FadeDistanceFar", Mathf.Lerp(startFogFar, target.fogFarDistance, t));

            // Smoothly morph the Cloud Material (From Image)
            cloudMaterial.SetColor("_BaseColor", Color.Lerp(startCloudBase, target.cloudBaseColor, t));
            cloudMaterial.SetColor("_EmissionColor", Color.Lerp(startCloudEmission, target.cloudEmissionColor, t));

            // Update Global Shader properties (Fluffy Clouds)
            Shader.SetGlobalFloat("_GlobalCloudDensity", Mathf.Lerp(Shader.GetGlobalFloat("_GlobalCloudDensity"), target.cloudDensity, t));
            Shader.SetGlobalColor("_GlobalCloudColor", Color.Lerp(Shader.GetGlobalColor("_GlobalCloudColor"), target.cloudColor, t));

            yield return null;
        }

        currentSeason = target;
        transitionCoroutine = null;
        Debug.Log($"Transition to {target.name} Complete.");
    }

    // Helper to snap values instantly (used at Start)
    private void ApplySeasonImmediate(SeasonConfiguration config)
    {
        sunLight.color = config.sunColor;
        sunLight.intensity = config.sunIntensity;
        sunLight.transform.rotation = Quaternion.Euler(config.middayRotation);

        fogMaterial.SetColor("_BaseColor", config.fogBaseColor);
        fogMaterial.SetColor("_EmissionColor", config.fogEmissionColor);
        fogMaterial.SetFloat("_FadeDistanceFar", config.fogFarDistance);

        cloudMaterial.SetColor("_BaseColor", config.cloudBaseColor);
        cloudMaterial.SetColor("_EmissionColor", config.cloudEmissionColor);

        if (normalGroup != null) normalGroup.SetActive(config.currentMode == CityMode.Normal);
        if (rainGroup != null) rainGroup.SetActive(config.currentMode == CityMode.Rain);
        if (dustGroup != null) dustGroup.SetActive(config.currentMode == CityMode.Dust);
    }
}