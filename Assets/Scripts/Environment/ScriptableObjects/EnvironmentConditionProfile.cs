// ReSharper disable All
#pragma warning disable 649
namespace Assets.Scripts.Environment.ScriptableObjects
{
  using MediaLab.NetworkEntities.Simpa;

  using UnityEngine;
  using UnityEngine.Rendering;

  [CreateAssetMenu]
  public class EnvironmentConditionProfile : ScriptableObject
  {
    [Header("Profile")]

    [SerializeField]
    private Constants.MomentsOfTheDayEnum dayTime;

    [Header("Post processing profile")]

    [SerializeField]
    private VolumeProfile postProcessingProfile;

    [Header("Lighting")]

    [SerializeField]
    private bool enableDirectionLight = true;

    [SerializeField]
    private Vector3 directionLightRotation;

    [SerializeField]
    private float lightIntensity;

    [SerializeField]
    private Color lightColor;

    [Space]

    [SerializeField]
    private LensFlareDataSRP lensFlareData;

    [SerializeField]
    private float lensFlareIntensity = 1f;

    [SerializeField]
    private float lensFlareScale = 1f;

    [Header("Emissive Artificial Lighting")]

    [SerializeField]
    private float artificialLightingIntensity;

    [SerializeField]
    private Color artificialLightingColor;

    [Header("Artificial Lighting")]

    [SerializeField]
    private bool enableArtificialLight;

    [Header("Particles")]

    [SerializeField]
    private Color dustParticleColor;

    public Constants.MomentsOfTheDayEnum DayTime => this.dayTime;

    public Vector3 DirectionLightRotation => this.directionLightRotation;

    public bool EnableDirectionLight => this.enableDirectionLight;

    public float LightIntensity => this.lightIntensity;

    public Color LightColor => this.lightColor;

    public LensFlareDataSRP LensFlareData => this.lensFlareData;

    public float LensFlareIntensity => this.lensFlareIntensity;

    public float LensFlareScale => this.lensFlareScale;

    public VolumeProfile PostProcessingProfile => this.postProcessingProfile;

    public float ArtificialLightingIntensity => this.artificialLightingIntensity;

    public Color ArtificialLightingColor => this.artificialLightingColor;

    public bool EnableArtificialLight => this.enableArtificialLight;

    public Color DustParticleColor => this.dustParticleColor;

    protected void OnValidate()
    {
      this.lightIntensity = Mathf.Max(0f, this.lightIntensity);
      this.artificialLightingIntensity = Mathf.Max(0f, this.artificialLightingIntensity);
    }
  }
}