using UnityEngine;

public enum CityMode { Normal, Rain, Dust }

[CreateAssetMenu(fileName = "NewSeasonConfig", menuName = "FourSeasons/SeasonConfig")]
public class SeasonConfiguration : ScriptableObject
{
    [Header("Mode Identity")]
    public CityMode currentMode;

    [Header("Environment Models")]
    public Material treeMaterial;

    [Header("Sky & Lighting")]
    public Material skyboxMaterial;
    public Color sunColor = Color.white;
    public float sunIntensity = 1.0f;

    [Header("Time of Day (Sun Rotations)")]
    public Vector3 middayRotation = new Vector3(90, -30, 0);

    [Header("Fog Material Settings")]
    [ColorUsage(true, true)]
    public Color fogBaseColor = Color.white;
    public Color fogEmissionColor;
    public float fogFarDistance = 20f;
    public float fogDistortionStrength = 0.1f;

    [Header("Cloud Material Settings (From Image)")]
    public Color cloudBaseColor = Color.white;
    [ColorUsage(true, true)]
    public Color cloudEmissionColor = Color.black;

    [Header("Global Cloud Shader (Fluffy)")]
    public float cloudDensity = 0.5f;
    public Color cloudColor = Color.white;

    [Header("Light")]
    public bool light;
}