using UnityEngine;

[CreateAssetMenu(fileName = "NewSeasonConfig", menuName = "FourSeasons/SeasonConfig")]
public class SeasonConfiguration : ScriptableObject
{
    [Header("Environment Models")]
    public Material treeMaterial;
    public Material buildingMaterial; // Optional: for snow-covered roofs

    [Header("Sky & Lighting")]
    public Material skyboxMaterial;
    public Color sunColor = Color.white;
    public float sunIntensity = 1.0f;

    [Header("Time of Day (Sun Rotations)")]
    public Vector3 middayRotation = new Vector3(90, 0, 0);
    public Vector3 nightRotation = new Vector3(-90, 0, 0);

    [Header("Fog Material Settings (From Image)")]
    [ColorUsage(true, true)] // Enables HDR for that glow
    public Color fogBaseColor = Color.white;
    public Color fogEmissionColor;
    public float fogFarDistance = 20f;
    public float fogDistortionStrength = 0.1f;

    [Header("Fluffy Cloud Settings")]
    public float cloudDensity = 0.5f;
    public Color cloudColor = Color.white;
}